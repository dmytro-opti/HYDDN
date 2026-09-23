/*
    TravellerAI - test data for development.

    Executed by DatabaseInitializer on application start in Development
    (Database:SeedTestData = true) right after migrations, or manually:
        sqlcmd -I -S localhost -U sa -d TravellerAI -i SeedTestData.sql

    The script is idempotent: it does nothing when the AspNetUsers table already has rows.
    Dates are relative to the current date (D = today).

    Test accounts (password for all: Traveller123! - public test credential, development only):
        olena.test@example.com   User         11111111-0000-0000-0000-000000000001
        andrii.test@example.com  User         11111111-0000-0000-0000-000000000002
        iryna.host@example.com   User, Admin  11111111-0000-0000-0000-000000000003

    Journeys:
        44444444-...-001  Olena  "Lviv weekend"    APPROVED, D+30 .. D+33, Ukraine
                          nights D+30, D+31 - Old Town Apartments; night D+32 - Grand Hotel Lviv (hotel switch on D+32)
                          days: D+30 Lviv centre walk, D+31 High Castle and coffee, D+32 Move to Grand Hotel, D+33 Opera evening
        44444444-...-002  Andrii "Krakow weekend"  draft: period and country set, no hotels in Poland yet
        44444444-...-003  Olena  "Carpathians"     draft: only details (continue with the period)

    Trips (catalog, one city each, start / finish at a hotel):
        55555555-...-001 Lviv centre walk (public)      Old Town Apartments -> Opera -> Potocki Palace -> Old Town Apartments
        55555555-...-002 High Castle and coffee (public) Old Town Apartments -> High Castle -> coffee tour -> Old Town Apartments
        55555555-...-003 Move to Grand Hotel (private)  Old Town Apartments -> Lychakiv Cemetery -> Grand Hotel Lviv
        55555555-...-004 Opera evening (public)         Grand Hotel Lviv -> Opera -> Grand Hotel Lviv
        55555555-...-005 Carpathian hike (public)       Carpathian Chalet -> hike -> Carpathian Chalet

    Enum values: JourneyStatus Active=0 | BookingStatus Pending=0, Confirmed=1, Cancelled=2, Completed=3
                 TransportType FLIGHT=0, TRAIN=1, BUS=2 | SeatClass Economy=0, Business=1
                 ActivityType ENTERTAINMENT=0, EDUCATION=1, SPORTS=2, TRAVEL=3 | FoodOptions None=0, BreakfastOnly=1, HalfBoard=2
                 TravelStyle Relaxed=0, Balanced=1, Active=2 | PersonalityType Introvert=0, Ambivert=1, Extrovert=2
                 BudgetLevel Low=0, Medium=1, High=2 | CompanionGender Any=0 | NotificationType Info=0, Journey=1, Booking=2
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF EXISTS (SELECT 1 FROM [AspNetUsers])
BEGIN
    PRINT 'Test data already exists - skipped';
    RETURN;
END;

DECLARE @now datetime2 = SYSUTCDATETIME();
DECLARE @today datetime2 = CAST(CAST(@now AS date) AS datetime2);

DECLARE @olena  uniqueidentifier = '11111111-0000-0000-0000-000000000001';
DECLARE @andrii uniqueidentifier = '11111111-0000-0000-0000-000000000002';
DECLARE @iryna  uniqueidentifier = '11111111-0000-0000-0000-000000000003';

DECLARE @ukraine uniqueidentifier = 'cccccccc-0000-0000-0000-000000000001';
DECLARE @poland  uniqueidentifier = 'cccccccc-0000-0000-0000-000000000002';

DECLARE @userRole  uniqueidentifier = '0f8fad5b-d9cb-469f-a165-70867728950e'; -- created by migration
DECLARE @adminRole uniqueidentifier = '7c9e6679-7425-40de-944b-e07fc1f90ae7'; -- created by migration
-- ASP.NET Core Identity V3 hash of the test password Traveller123!
DECLARE @passwordHash nvarchar(max) = N'AQAAAAIAAYagAAAAEIJVREqpkxXoMw++WjRBetYzNMwaxRjYl91dne4nV7xZVWo40pVSVMNe+hUgNkZfIA==';

DECLARE @lviv          uniqueidentifier = '22222222-0000-0000-0000-000000000001';
DECLARE @kyiv          uniqueidentifier = '22222222-0000-0000-0000-000000000002';
DECLARE @yaremche      uniqueidentifier = '22222222-0000-0000-0000-000000000003';
DECLARE @krakow        uniqueidentifier = '22222222-0000-0000-0000-000000000004';
DECLARE @lvivOpera     uniqueidentifier = '22222222-0000-0000-0000-000000000005';
DECLARE @lvivCastle    uniqueidentifier = '22222222-0000-0000-0000-000000000006';
DECLARE @lvivLychakiv  uniqueidentifier = '22222222-0000-0000-0000-000000000007';
DECLARE @lvivPotocki   uniqueidentifier = '22222222-0000-0000-0000-000000000008';
DECLARE @hotelOldTown  uniqueidentifier = '22222222-0000-0000-0000-000000000009';
DECLARE @hotelGrand    uniqueidentifier = '22222222-0000-0000-0000-000000000010';
DECLARE @hotelChalet   uniqueidentifier = '22222222-0000-0000-0000-000000000011';
DECLARE @hotelKyiv     uniqueidentifier = '22222222-0000-0000-0000-000000000012';

DECLARE @placeOldTown uniqueidentifier = '33333333-0000-0000-0000-000000000001';
DECLARE @placeChalet  uniqueidentifier = '33333333-0000-0000-0000-000000000002';
DECLARE @placeKyiv    uniqueidentifier = '33333333-0000-0000-0000-000000000003';
DECLARE @placeGrand   uniqueidentifier = '33333333-0000-0000-0000-000000000004';

DECLARE @actCoffee uniqueidentifier = '77777777-0000-0000-0000-000000000001';
DECLARE @actHike   uniqueidentifier = '77777777-0000-0000-0000-000000000002';
DECLARE @actWawel  uniqueidentifier = '77777777-0000-0000-0000-000000000003';

DECLARE @journeyLviv   uniqueidentifier = '44444444-0000-0000-0000-000000000001';
DECLARE @journeyKrakow uniqueidentifier = '44444444-0000-0000-0000-000000000002';
DECLARE @journeyCarpat uniqueidentifier = '44444444-0000-0000-0000-000000000003';
DECLARE @budgetLviv    uniqueidentifier = '88888888-0000-0000-0000-000000000001';

DECLARE @bookingOldTown uniqueidentifier = '66666666-0000-0000-0000-000000000001';
DECLARE @bookingGrand   uniqueidentifier = '66666666-0000-0000-0000-000000000002';
DECLARE @bookingKyiv    uniqueidentifier = '66666666-0000-0000-0000-000000000003';

DECLARE @tripLvivWalk       uniqueidentifier = '55555555-0000-0000-0000-000000000001';
DECLARE @tripCastleCoffee   uniqueidentifier = '55555555-0000-0000-0000-000000000002';
DECLARE @tripMoveGrand      uniqueidentifier = '55555555-0000-0000-0000-000000000003';
DECLARE @tripOperaEvening   uniqueidentifier = '55555555-0000-0000-0000-000000000004';
DECLARE @tripCarpathianHike uniqueidentifier = '55555555-0000-0000-0000-000000000005';

BEGIN TRANSACTION;

-- Identity users (login = email) and roles
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash],
                           [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled],
                           [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
VALUES
    (@olena,  N'olena.test@example.com',  N'OLENA.TEST@EXAMPLE.COM',  N'olena.test@example.com',  N'OLENA.TEST@EXAMPLE.COM',  1,
     @passwordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), NULL, 0, 0, NULL, 1, 0),
    (@andrii, N'andrii.test@example.com', N'ANDRII.TEST@EXAMPLE.COM', N'andrii.test@example.com', N'ANDRII.TEST@EXAMPLE.COM', 1,
     @passwordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), NULL, 0, 0, NULL, 1, 0),
    (@iryna,  N'iryna.host@example.com',  N'IRYNA.HOST@EXAMPLE.COM',  N'iryna.host@example.com',  N'IRYNA.HOST@EXAMPLE.COM',  0,
     @passwordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), NULL, 0, 0, NULL, 1, 0);

INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
VALUES (@olena, @userRole), (@andrii, @userRole), (@iryna, @userRole), (@iryna, @adminRole);

-- User profiles (same ids as identity users)
INSERT INTO [Users] ([Id], [Name], [FirstName], [LastName], [Email], [Created], [Updated])
VALUES
    (@olena,  N'olena.k',  N'Olena',  N'Kovalenko', N'olena.test@example.com',  @now, @now),
    (@andrii, N'andrii.m', N'Andrii', N'Melnyk',    N'andrii.test@example.com', @now, @now),
    (@iryna,  N'iryna.b',  N'Iryna',  N'Bondar',    N'iryna.host@example.com',  @now, @now);

-- Countries
INSERT INTO [Countries] ([Id], [Name], [Code], [Created], [Updated])
VALUES
    (@ukraine, N'Ukraine', N'UA', @now, @now),
    (@poland,  N'Poland',  N'PL', @now, @now);

-- Locations (WGS 84 coordinates); hotels have their own locations - they are the first / last stops of day trips
INSERT INTO [Locations] ([Id], [Name], [CountryId], [City], [Street], [ZipCode], [Latitude], [Longitude], [Created], [Updated])
VALUES
    (@lviv, N'Rynok Square', @ukraine, N'Lviv', N'Rynok Square 1', N'79008', 49.841900, 24.031600, @now, @now),
    (@kyiv, N'Khreshchatyk', @ukraine, N'Kyiv', N'Khreshchatyk St 22', N'01001', 50.447400, 30.522600, @now, @now),
    (@yaremche, N'Yaremche centre', @ukraine, N'Yaremche', N'Svobody St 5', N'78500', 48.458300, 24.555000, @now, @now),
    (@krakow, N'Wawel Royal Castle', @poland, N'Krakow', N'Wawel 5', N'31-001', 50.054000, 19.935400, @now, @now),
    (@lvivOpera, N'Lviv Opera House', @ukraine, N'Lviv', N'Svobody Ave 28', N'79000', 49.844000, 24.026200, @now, @now),
    (@lvivCastle, N'High Castle', @ukraine, N'Lviv', N'Vysokyi Zamok St', N'79008', 49.848300, 24.039500, @now, @now),
    (@lvivLychakiv, N'Lychakiv Cemetery', @ukraine, N'Lviv', N'Mechnykova St 33', N'79013', 49.831600, 24.055800, @now, @now),
    (@lvivPotocki, N'Potocki Palace', @ukraine, N'Lviv', N'Kopernyka St 15', N'79000', 49.836500, 24.022600, @now, @now),
    (@hotelOldTown, N'Old Town Apartments', @ukraine, N'Lviv', N'Rynok Square 10', N'79008', 49.842500, 24.032700, @now, @now),
    (@hotelGrand, N'Grand Hotel Lviv', @ukraine, N'Lviv', N'Svobody Ave 13', N'79000', 49.840800, 24.028200, @now, @now),
    (@hotelChalet, N'Carpathian Chalet', @ukraine, N'Yaremche', N'Hutsulska St 12', N'78500', 48.455000, 24.553000, @now, @now),
    (@hotelKyiv, N'Kyiv Riverside Hotel', @ukraine, N'Kyiv', N'Naberezhne Hwy 2', N'01001', 50.450100, 30.523400, @now, @now);

-- Places (hotels, owned by the host)
INSERT INTO [Places] ([Id], [Name], [Description], [Type], [Address], [PricePerHour], [Capacity], [IsAvailable], [Status],
                      [AverageRating], [ImageUrls], [Food], [BookingRules], [LocationId], [OwnerId], [Created], [Updated])
VALUES
    (@placeOldTown, N'Old Town Apartments', N'Cozy apartments near Rynok Square', N'Apartment', N'Rynok Square 10, Lviv',
     4.50, 4, 1, 0, 4.7, N'["https://example.com/img/lviv-1.jpg"]', 0, N'Check-in after 14:00', @hotelOldTown, @iryna, @now, @now),
    (@placeChalet,  N'Carpathian Chalet', N'Wooden chalet with mountain view', N'Chalet', N'Hutsulska St 12, Yaremche',
     6.25, 6, 1, 0, 4.9, N'["https://example.com/img/chalet-1.jpg"]', 2, N'No smoking', @hotelChalet, @iryna, @now, @now),
    (@placeKyiv,    N'Kyiv Riverside Hotel', N'Business hotel on the Dnipro embankment', N'Hotel', N'Naberezhne Hwy 2, Kyiv',
     8.00, 2, 1, 0, 4.3, N'[]', 1, NULL, @hotelKyiv, @iryna, @now, @now),
    (@placeGrand,   N'Grand Hotel Lviv', N'Classic hotel on Svobody Avenue', N'Hotel', N'Svobody Ave 13, Lviv',
     6.00, 3, 1, 0, 4.6, N'[]', 1, NULL, @hotelGrand, @iryna, @now, @now);

-- Activities (the address is used as a trip stop)
INSERT INTO [Activities] ([Id], [Name], [Type], [Description], [Price], [Status], [Rating], [ImageUrl], [VideoUrl],
                          [Period_Start], [Period_End], [LocationId], [Created], [Updated])
VALUES
    (@actCoffee, N'Lviv coffee mine tour', 0, N'Guided tour with coffee tasting', 12.00, 0, 4.8, N'https://example.com/img/coffee.jpg', NULL, NULL, NULL, @lviv, @now, @now),
    (@actHike,   N'Carpathian forest hike', 2, N'Guided hike from Yaremche', 35.00, 0, 4.9, NULL, NULL, NULL, NULL, @yaremche, @now, @now),
    (@actWawel,  N'Wawel castle tour', 1, N'Royal castle and cathedral', 20.00, 0, 4.6, NULL, NULL, NULL, NULL, @krakow, @now, @now);

-- Reviews (each review targets a place or an activity)
INSERT INTO [Reviews] ([Id], [UserId], [PlaceId], [ActivityId], [Rating], [Comment], [Title], [IsVisible], [LikesCount], [Created], [Updated])
VALUES
    ('aaaaaaaa-0000-0000-0000-000000000001', @andrii, @placeOldTown, NULL, 5, N'Great location, very clean', N'Perfect stay', 1, 3, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000002', @andrii, @placeKyiv, NULL, 4, N'Good for business trips', N'Solid hotel', 1, 1, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000003', @olena, NULL, @actCoffee, 5, N'Loved the coffee tasting', N'Must visit', 1, 7, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000004', @andrii, NULL, @actHike, 4, N'Hard but beautiful', N'Worth it', 1, 2, @now, @now);

-- Trips catalog (distances: great-circle, km)
INSERT INTO [Trips] ([Id], [Name], [Description], [Rating], [IsPublic], [CountryId], [City], [DistanceKm], [UserId], [Created], [Updated])
VALUES
    (@tripLvivWalk, N'Lviv centre walk', N'Opera House and Potocki Palace', 4.5, 1, @ukraine, N'Lviv', 2.35, @iryna, @now, @now),
    (@tripCastleCoffee, N'High Castle and coffee', N'View from the High Castle and coffee tasting', 4.6, 1, @ukraine, N'Lviv', 1.82, @olena, @now, @now),
    (@tripMoveGrand, N'Move to Grand Hotel', N'Lychakiv Cemetery on the way to the new hotel', 4.7, 0, @ukraine, N'Lviv', 4.28, @olena, @now, @now),
    (@tripOperaEvening, N'Opera evening', N'Evening walk from the Grand Hotel', 4.8, 1, @ukraine, N'Lviv', 0.76, @iryna, @now, @now),
    (@tripCarpathianHike, N'Carpathian hike', N'One day hike from the chalet', 4.9, 1, @ukraine, N'Yaremche', 0.80, @iryna, @now, @now);

INSERT INTO [TripStops] ([Id], [TripId], [Order], [LocationId], [ActivityId], [DistanceFromPreviousKm], [Created], [Updated])
VALUES
    ('eeeeeeee-0000-0000-0001-000000000001', @tripLvivWalk, 0, @hotelOldTown, NULL, 0.00, @now, @now),
    ('eeeeeeee-0000-0000-0001-000000000002', @tripLvivWalk, 1, @lvivOpera, NULL, 0.50, @now, @now),
    ('eeeeeeee-0000-0000-0001-000000000003', @tripLvivWalk, 2, @lvivPotocki, NULL, 0.87, @now, @now),
    ('eeeeeeee-0000-0000-0001-000000000004', @tripLvivWalk, 3, @hotelOldTown, NULL, 0.98, @now, @now),
    ('eeeeeeee-0000-0000-0002-000000000001', @tripCastleCoffee, 0, @hotelOldTown, NULL, 0.00, @now, @now),
    ('eeeeeeee-0000-0000-0002-000000000002', @tripCastleCoffee, 1, @lvivCastle, NULL, 0.81, @now, @now),
    ('eeeeeeee-0000-0000-0002-000000000003', @tripCastleCoffee, 2, @lviv, @actCoffee, 0.91, @now, @now),
    ('eeeeeeee-0000-0000-0002-000000000004', @tripCastleCoffee, 3, @hotelOldTown, NULL, 0.10, @now, @now),
    ('eeeeeeee-0000-0000-0003-000000000001', @tripMoveGrand, 0, @hotelOldTown, NULL, 0.00, @now, @now),
    ('eeeeeeee-0000-0000-0003-000000000002', @tripMoveGrand, 1, @lvivLychakiv, NULL, 2.05, @now, @now),
    ('eeeeeeee-0000-0000-0003-000000000003', @tripMoveGrand, 2, @hotelGrand, NULL, 2.23, @now, @now),
    ('eeeeeeee-0000-0000-0004-000000000001', @tripOperaEvening, 0, @hotelGrand, NULL, 0.00, @now, @now),
    ('eeeeeeee-0000-0000-0004-000000000002', @tripOperaEvening, 1, @lvivOpera, NULL, 0.38, @now, @now),
    ('eeeeeeee-0000-0000-0004-000000000003', @tripOperaEvening, 2, @hotelGrand, NULL, 0.38, @now, @now),
    ('eeeeeeee-0000-0000-0005-000000000001', @tripCarpathianHike, 0, @hotelChalet, NULL, 0.00, @now, @now),
    ('eeeeeeee-0000-0000-0005-000000000002', @tripCarpathianHike, 1, @yaremche, @actHike, 0.40, @now, @now),
    ('eeeeeeee-0000-0000-0005-000000000003', @tripCarpathianHike, 2, @hotelChalet, NULL, 0.40, @now, @now);

-- Journeys: approved (fully planned) and two drafts to continue
INSERT INTO [Budgets] ([Id], [Budget], [Total], [CurrencyCode], [Created], [Updated])
VALUES (@budgetLviv, 1500.00, 434.00, N'USD', @now, @now); -- hotels 360.00 + train 50 + coffee tour 12 x 2 members

INSERT INTO [Journeys] ([Id], [Status], [Title], [Description], [Period_Start], [Period_End], [Members], [Approved], [ApprovedAt],
                        [CountryId], [BudgetId], [UserId], [Created], [Updated])
VALUES
    (@journeyLviv,   0, N'Lviv weekend', N'Old town, High Castle and opera', DATEADD(day, 30, @today), DATEADD(day, 33, @today),
     N'["Olena","Taras"]', 1, @now, @ukraine, @budgetLviv, @olena, @now, @now),
    (@journeyKrakow, 0, N'Krakow weekend', N'Short city break', DATEADD(day, 60, @today), DATEADD(day, 63, @today),
     N'["Andrii"]', 0, NULL, @poland, NULL, @andrii, @now, @now),
    (@journeyCarpat, 0, N'Carpathians', N'Hiking week', NULL, NULL, N'[]', 0, NULL, NULL, NULL, @olena, @now, @now);

-- Hotel stays: nights [check-in, check-out); frozen because the journey is approved
INSERT INTO [Bookings] ([Id], [UserId], [JourneyId], [PropertyId], [RoomId], [Period_Start], [Period_End], [TotalPrice], [Currency],
                        [IsPaid], [PaymentMethod], [Adults], [Children], [Status], [IsFrozen], [CheckInDate], [CheckOutDate], [BudgetId],
                        [Created], [Updated])
VALUES
    (@bookingOldTown, @olena, @journeyLviv, @placeOldTown, NULL, DATEADD(day, 30, @today), DATEADD(day, 32, @today), 216.00, N'USD',
     0, NULL, 2, 0, 0, 1, DATEADD(day, 30, @today), DATEADD(day, 32, @today), NULL, @now, @now),
    (@bookingGrand,   @olena, @journeyLviv, @placeGrand, NULL, DATEADD(day, 32, @today), DATEADD(day, 33, @today), 144.00, N'USD',
     1, N'card', 2, 0, 1, 1, DATEADD(day, 32, @today), DATEADD(day, 33, @today), NULL, @now, @now),
    (@bookingKyiv,    @andrii, NULL, @placeKyiv, NULL, DATEADD(day, -20, @today), DATEADD(day, -17, @today), 576.00, N'USD',
     1, N'card', 1, 0, 3, 1, DATEADD(day, -20, @today), DATEADD(day, -17, @today), NULL, @now, @now);

-- Journey days (one row per date of the period)
INSERT INTO [JourneyDays] ([Id], [JourneyId], [Date], [TripId], [Created], [Updated])
VALUES
    ('ffffffff-0000-0000-0001-000000000001', @journeyLviv, DATEADD(day, 30, @today), @tripLvivWalk, @now, @now),
    ('ffffffff-0000-0000-0001-000000000002', @journeyLviv, DATEADD(day, 31, @today), @tripCastleCoffee, @now, @now),
    ('ffffffff-0000-0000-0001-000000000003', @journeyLviv, DATEADD(day, 32, @today), @tripMoveGrand, @now, @now),
    ('ffffffff-0000-0000-0001-000000000004', @journeyLviv, DATEADD(day, 33, @today), @tripOperaEvening, @now, @now),
    ('ffffffff-0000-0000-0002-000000000001', @journeyKrakow, DATEADD(day, 60, @today), NULL, @now, @now),
    ('ffffffff-0000-0000-0002-000000000002', @journeyKrakow, DATEADD(day, 61, @today), NULL, @now, @now),
    ('ffffffff-0000-0000-0002-000000000003', @journeyKrakow, DATEADD(day, 62, @today), NULL, @now, @now),
    ('ffffffff-0000-0000-0002-000000000004', @journeyKrakow, DATEADD(day, 63, @today), NULL, @now, @now);

-- Journey transports (Duration in ticks: 1 hour = 36000000000)
INSERT INTO [Transports] ([Id], [JourneyId], [Type], [Company], [Price], [Period_Start], [Period_End], [SeatClass], [SeatCount],
                          [Duration], [Created], [Updated])
VALUES
    ('bbbbbbbb-0000-0000-0000-000000000001', @journeyLviv, 1, N'Ukrzaliznytsia', 50.00,
     DATEADD(hour, 30*24 + 6, @today), DATEADD(hour, 30*24 + 11, @today), 0, 2, 5 * CAST(36000000000 AS bigint), @now, @now),
    ('bbbbbbbb-0000-0000-0000-000000000002', @journeyKrakow, 0, N'LOT Polish Airlines', 120.00,
     DATEADD(hour, 60*24 + 7, @today), DATEADD(minute, 60*24*60 + 7*60 + 75, @today), 0, 1, CAST(45000000000 AS bigint), @now, @now);

-- Travel profiles
INSERT INTO [UserInfos] ([Id], [UserId], [BirthDate], [TravelStyle], [PersonalityType], [BudgetLevel], [CompanionGender],
                         [LookingFor], [Languages], [Interests], [Created], [Updated])
VALUES
    ('99999999-0000-0000-0000-000000000001', @olena,  '1997-05-14', 2, 2, 1, 0, N'Travel companions for hiking weekends', N'["uk","en"]', N'[2,3]', @now, @now),
    ('99999999-0000-0000-0000-000000000002', @andrii, '1992-11-02', 0, 0, 2, 0, N'Calm city breaks with museums', N'["uk","en","pl"]', N'[1,0]', @now, @now);

INSERT INTO [UserInfoChosenActivities] ([UserInfoId], [ActivityId])
VALUES ('99999999-0000-0000-0000-000000000001', @actHike), ('99999999-0000-0000-0000-000000000001', @actCoffee),
       ('99999999-0000-0000-0000-000000000002', @actWawel);

INSERT INTO [UserInfoChosenTrips] ([UserInfoId], [TripId])
VALUES ('99999999-0000-0000-0000-000000000001', @tripCarpathianHike), ('99999999-0000-0000-0000-000000000002', @tripLvivWalk);

INSERT INTO [UserInfoPreferredCountries] ([UserInfoId], [CountryId])
VALUES ('99999999-0000-0000-0000-000000000001', @ukraine), ('99999999-0000-0000-0000-000000000002', @poland),
       ('99999999-0000-0000-0000-000000000002', @ukraine);

-- Notifications
INSERT INTO [Notifications] ([Id], [UserId], [Type], [Title], [Message], [IsRead], [ReadAt], [Created], [Updated])
VALUES
    ('dddddddd-0000-0000-0000-000000000001', @olena,  1, N'Journey approved', N'Lviv weekend setup is finished.', 1, @now, @now, @now),
    ('dddddddd-0000-0000-0000-000000000002', @olena,  2, N'Booking confirmed', N'Grand Hotel Lviv booking was paid.', 0, NULL, @now, @now),
    ('dddddddd-0000-0000-0000-000000000003', @andrii, 0, N'Welcome to TravellerAI', N'Fill in your travel profile.', 0, NULL, @now, @now);

COMMIT TRANSACTION;

PRINT 'Test data inserted';
