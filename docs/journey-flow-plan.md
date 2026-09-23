# Journey flow — gap analysis and implementation plan

Goal: a client builds a journey step by step and publishes it:

1. **Create** the journey (title, description) — it starts as a *Draft*.
2. **Set the period** (start and end dates).
3. **Select the country.**
4. **Plan the days**: for every day pick an *available trip* in the country or **create a new trip**.
   A trip is a one-day route between locations (ordered stops, optimized on the map).
5. **Select a hotel** for the journey period.
6. **Save and publish** — only when every step is valid.

Decisions already taken:

| Topic | Decision |
|---|---|
| Auth | ASP.NET Core Identity + JWT access / refresh tokens (implemented) |
| Users | `ApplicationUser` (credentials) and `UserEntity` (profile) share the Id (implemented) |
| Trips | **Shared route catalog**: a trip is a reusable day route in a country; a journey schedules trips by day (`JourneyDay`) |

## 1. What exists today

Implemented and verified end to end (Swagger: `/swagger`):

| Area | Endpoints |
|---|---|
| Auth | `POST /api/auth/register`, `login`, `refresh`, `logout`, `GET /api/auth/me`, `PUT /api/auth/password` |
| Users | `GET/PUT /api/users/me/profile`, `PUT /api/users/me/email` |
| Journeys | `POST /api/journeys`, `GET /api/journeys/{id}/status` |
| Trips (old model) | `GET /api/trips/{id}/status`, `PUT /api/trips/{id}`, `POST /api/trips/{id}/build`, `POST /api/trips/{id}/transports`, `POST /api/trips/{id}/bookings` |
| Bookings | `PUT /api/bookings/{id}`, `POST /api/bookings/{id}/select` |
| Locations | `GET /api/locations?country=&city=` (anonymous) |

Services implemented but **not exposed** by any command / endpoint:

| Service | Methods without a command |
|---|---|
| `IJourneyService` | `DeleteJourney`, `SelectPeriod`*, `SetMembers`* (*only inside BuildTrip) |
| `ITripService` | `CreateTrip`, `DeleteTrip`, `AddPeriodTrip` |
| `IBookingService` | `IsPlaceAvailableAsync`, `CreateBookingAsync`, `ChangeDatesAsync`, `PayAsync` |
| `IMapService` | `CreateMapAsync`, `BuildOptimalWayAsync`, `GetAvailableLocationsAsync`, `SelectLocationAsync` |
| `ITransportService` | `SearchTransportsAsync`, `SelectTransports`, `SelectAvailableTransports` |
| `INotificationService` | `GetNotificationsAsync`, `MarkAsReadAsync`, `DeleteNotificationAsync` |
| `IUserService` | `RemoveUserAsync`, `UpdateNameAsync` |

## 2. Gaps against the journey flow

| Step | Gap |
|---|---|
| 1 Create | Status is `Active` — there is no *Draft / Published* lifecycle. No list / details / delete endpoints. |
| 2 Period | Service exists, no command / endpoint. No rule for days outside a changed period. |
| 3 Country | `Journey` has no `CountryId`. No countries endpoint. |
| 4 Days / trips | `Trip` is a user-owned, journey-bound object with period, booking, budget. It has **no stops** and no country. There is no `JourneyDay`. No catalog search, no route creation, no map endpoints. |
| 5 Hotel | The booking is attached to a **trip** (`Trip.BookingId`), not to the journey. No hotel search (place type, country, availability). |
| 6 Publish | No publish command, no validation checklist, no progress endpoint, no notifications. |
| Cross-cutting | No notifications API, no admin catalog management (countries, locations, places, activities), list endpoints load whole graphs via lazy loading (no projections / paging). |

## 3. Target domain model

```
Journey (Draft | Published | Completed | Cancelled)
  ├─ UserId, Title, Description, Period, CountryId ─► Country
  ├─ Days: JourneyDay (Date, DayNumber, TripId?) ─► Trip (catalog route)
  ├─ HotelBookingId? ─► Booking (Place of type Hotel, covers the period)
  ├─ Transports (to the country / between cities) — optional
  └─ Budget (limit, calculated total)

Trip (catalog day route)
  ├─ Name, Description, CountryId, City?, AuthorId, IsPublic
  ├─ Stops: TripStop (Order, LocationId, ActivityId?, PlannedMinutes?)
  └─ DistanceKm (from IMapService)
```

Entity changes (one migration, `JourneyFlow`):

| Entity | Change |
|---|---|
| `JourneyStatus` | `Draft=0, Published=1, Completed=2, Cancelled=3` (data migration: `Active → Draft`, `Passed → Completed`, `Failed → Cancelled`) |
| `JourneyEntity` | + `CountryId?`, `HotelBookingId?`, `PublishedAt?`; + `Days` collection |
| `JourneyDayEntity` (new) | `JourneyId`, `Date`, `DayNumber`, `TripId?`, `Notes?`; unique `(JourneyId, Date)`; cascade from journey, restrict from trip |
| `TripEntity` | + `CountryId`, `City?`, `Description?`, `IsPublic`, `DistanceKm`, `Stops`; `UserId` becomes the author. Move out: `Period`, `BookingId`, `BudgetId`, `JourneyId`, `Status` (they belong to the journey / day) |
| `TripStopEntity` (new) | `TripId`, `Order`, `LocationId`, `ActivityId?`, `PlannedMinutes?`; unique `(TripId, Order)` |
| `PlaceEntity` | `Type` string → `PlaceType` enum (`Hotel`, `Apartment`, `Hostel`, `Chalet`, ...) |
| `TransportEntity` | `TripId` → `JourneyId` (required); optional `FromLocationId` / `ToLocationId` |

The old trip-level commands (`BuildTrip`, `UpdateTrip` booking part, trip `AddBooking`, `SelectBooking`) become obsolete and are replaced by the journey steps below.

## 4. Commands and endpoints to implement

All endpoints are `[Authorize]`; the user comes from the JWT. A journey can be edited only while it is a *Draft* and only by its owner (`EnsureOwnerAsync` + status check → 403 / 409).

### Phase 1 — journey wizard core (steps 1–3, 6 skeleton)

| # | Command | Endpoint | Service | Rules |
|---|---|---|---|---|
| 1 | `CreateJourneyCommand` (rename `BuildJourneyCommand`) | `POST /api/journeys` | `CreateJourney` → status *Draft* | title required |
| 2 | `GetMyJourneysCommand` | `GET /api/journeys?status=&page=` | new `GetUserJourneysAsync` (projection, paging) | own journeys only |
| 3 | `GetJourneyCommand` | `GET /api/journeys/{id}` | `GetJourneyAsync` → `JourneyDetailsViewModel` (days, trips, hotel, budget) | owner |
| 4 | `DeleteJourneyCommand` | `DELETE /api/journeys/{id}` | `DeleteJourney` | Draft only |
| 5 | `SetJourneyPeriodCommand` | `PUT /api/journeys/{id}/period` | `SelectPeriod` + create / remove `JourneyDay` rows | start ≥ today, max length (constant), removing days with trips needs `force=true` |
| 6 | `GetCountriesCommand` | `GET /api/countries` (anonymous) | new `ICountryService.GetCountriesAsync` | |
| 7 | `SelectJourneyCountryCommand` | `PUT /api/journeys/{id}/country` | new `SelectCountryAsync` | changing the country clears day trips and hotel (confirm with `force=true`) |
| 8 | `GetJourneyProgressCommand` | `GET /api/journeys/{id}/progress` | new `GetProgressAsync` | returns every step with `completed` flag and errors — drives the client wizard |

### Phase 2 — day routes (step 4)

| # | Command | Endpoint | Service | Rules |
|---|---|---|---|---|
| 9 | `SearchTripsCommand` | `GET /api/countries/{countryId}/trips?city=&interest=&page=` | new `ITripService.SearchCatalogAsync` | public trips + own private trips, ordered by rating / distance |
| 10 | `GetTripCommand` | `GET /api/trips/{id}` | `GetTripAsync` → stops + map | public or own |
| 11 | `CreateTripCommand` | `POST /api/trips` | `CreateTrip` (new shape: country, stops) | 2..N stops (constant), all stops in the trip country, locations need coordinates |
| 12 | `UpdateTripStopsCommand` | `PUT /api/trips/{id}/stops` | new `SetStopsAsync` | author only; used trips are copied instead of changed (copy-on-write) |
| 13 | `OptimizeTripRouteCommand` | `POST /api/trips/{id}/optimize` | `IMapService.BuildOptimalWayAsync` → saves stop order + `DistanceKm` | keeps the first stop |
| 14 | `GetTripMapCommand` | `GET /api/trips/{id}/map` | `IMapService.CreateMapAsync` | |
| 15 | `GetMapLocationsCommand` | `GET /api/countries/{countryId}/locations?city=` | `IMapService.GetAvailableLocationsAsync` | locations with coordinates |
| 16 | `AssignDayTripCommand` | `PUT /api/journeys/{id}/days/{date}` `{ tripId }` | new `AssignTripAsync` | date inside the period, trip in the journey country |
| 17 | `ClearDayTripCommand` | `DELETE /api/journeys/{id}/days/{date}` | new `ClearTripAsync` | |
| 18 | `DeleteTripCommand` | `DELETE /api/trips/{id}` | `DeleteTrip` | author only, not used by published journeys |

### Phase 3 — hotel (step 5)

| # | Command | Endpoint | Service | Rules |
|---|---|---|---|---|
| 19 | `SearchHotelsCommand` | `GET /api/journeys/{id}/hotels?city=&maxPrice=` | new `IPlaceService.SearchAvailableAsync` + `IsPlaceAvailableAsync` | places of type Hotel/Apartment in the journey country, capacity ≥ members, free for the whole period, price for the stay |
| 20 | `SelectJourneyHotelCommand` | `PUT /api/journeys/{id}/hotel` `{ placeId, adults, children }` | `CreateBookingAsync` (period = journey period) and link `HotelBookingId`; previous pending booking is cancelled | available, capacity |
| 21 | `RemoveJourneyHotelCommand` | `DELETE /api/journeys/{id}/hotel` | cancel booking | not paid |
| 22 | `PayBookingCommand` | `POST /api/bookings/{id}/pay` `{ paymentMethod }` | `PayAsync` | owner; card data goes only to a payment provider (future) |
| 23 | `ChangeBookingDatesCommand` | `PUT /api/bookings/{id}/dates` | `ChangeDatesAsync` | used when the journey period changes |

### Phase 4 — publish (step 6) and lifecycle

| # | Command | Endpoint | Service | Rules |
|---|---|---|---|---|
| 24 | `PublishJourneyCommand` | `POST /api/journeys/{id}/publish` | new `PublishAsync` | all progress steps complete: period in the future, country, every day has a trip (or explicitly free day), hotel booked for the whole period and still available; recalculates budget; freezes the hotel booking; status → *Published*; notification |
| 25 | `UnpublishJourneyCommand` | `POST /api/journeys/{id}/unpublish` | new | back to *Draft* before the start date; unfreezes the booking |
| 26 | `CancelJourneyCommand` | `POST /api/journeys/{id}/cancel` | new | cancels bookings, notification |
| 27 | background job | — | `CompleteFinishedJourneys` (hosted service, daily) | *Published* with end date in the past → *Completed* |

### Phase 5 — supporting APIs

| # | Command | Endpoint | Service |
|---|---|---|---|
| 28 | `GetNotificationsCommand` | `GET /api/notifications?unreadOnly=` | `GetNotificationsAsync` |
| 29 | `MarkNotificationReadCommand` | `PUT /api/notifications/{id}/read` | `MarkAsReadAsync` |
| 30 | `DeleteNotificationCommand` | `DELETE /api/notifications/{id}` | `DeleteNotificationAsync` |
| 31 | `SearchTransportsCommand` | `GET /api/transports/search?type=&maxPrice=&from=&to=` | `SearchTransportsAsync` (later: external provider) |
| 32 | `AddJourneyTransportCommand` | `POST /api/journeys/{id}/transports` | `AddTransportAsync` (journey-level) |
| 33 | `DeleteAccountCommand` | `DELETE /api/users/me` | `RemoveUserAsync` (requires password confirmation) |
| 34 | Admin catalog commands | `POST/PUT/DELETE /api/admin/{countries,locations,places,activities}` | `[Authorize(Roles = "Admin")]`, generic repositories |

### Phase 6 — auth follow-ups

| # | Item |
|---|---|
| 35 | Email confirmation: `AddDefaultTokenProviders`, `IEmailSender`, `POST /api/auth/confirm-email`, `POST /api/auth/resend-confirmation` |
| 36 | Password reset: `POST /api/auth/forgot-password`, `POST /api/auth/reset-password` |
| 37 | Require confirmed email for publishing |

## 5. Wizard progress contract

`GET /api/journeys/{id}/progress`

```json
{
  "journeyId": "…",
  "status": "Draft",
  "canPublish": false,
  "steps": [
    { "step": "Details", "completed": true,  "errors": [] },
    { "step": "Period",  "completed": true,  "errors": [] },
    { "step": "Country", "completed": true,  "errors": [] },
    { "step": "Days",    "completed": false, "errors": ["2026-10-25 has no trip"] },
    { "step": "Hotel",   "completed": false, "errors": ["Hotel is not selected"] }
  ]
}
```

The same checks are reused by `PublishJourneyCommand`, so the wizard and publishing never disagree.

## 6. Order of work and estimates

| Phase | Content | Estimate |
|---|---|---|
| 0 | Migration `JourneyFlow` (entities above), seed update, remove obsolete trip commands | 1–1.5 d |
| 1 | Journey wizard core, countries, progress | 1.5 d |
| 2 | Trip catalog, stops, map, day assignment | 2–3 d |
| 3 | Hotel search / selection, pay | 1.5 d |
| 4 | Publish / unpublish / cancel, completion job | 1 d |
| 5 | Notifications, transports, account deletion, admin catalog | 1.5–2 d |
| 6 | Email confirmation, password reset | 1 d |
| — | Integration tests (WebApplicationFactory + SQL container) for every phase | continuous |

## 7. Technical notes

- **Queries**: list / search endpoints should use projections (`Select` into models) with paging instead of lazy-loading whole graphs.
- **Concurrency**: publishing and hotel selection check availability again inside a transaction; add a `rowversion` to `Booking` to avoid double booking under load.
- **Constants**: new limits (max journey days, stops per trip, page size) go to `Constants.Validation`.
- **Existing dev databases**: the `IdentityAndProfileRelations` migration adds required foreign keys (profiles → identity users, locations → countries); a dev database with old data must be recreated (the app does it automatically in Development on an empty database).
