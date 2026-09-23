/*
    TravellerAI - test data for development.

    Executed by DatabaseInitializer on application start in Development
    (Database:SeedTestData = true) right after migrations, or manually:
        sqlcmd -I -S localhost -U sa -d TravellerAI -i SeedTestData.sql

    The script is idempotent: it does nothing when the AspNetUsers table already has rows.

    Test accounts (password for all: Traveller123! - public test credential, development only):
        olena.test@example.com   User
        andrii.test@example.com  User
        iryna.host@example.com   User, Admin
    Dates are relative to the current date, so bookings and trips stay in the future.
    Ids are fixed to be easily used in Swagger / manual testing:

        Users       Olena (traveller)  11111111-0000-0000-0000-000000000001
                    Andrii (traveller) 11111111-0000-0000-0000-000000000002
                    Iryna (host)       11111111-0000-0000-0000-000000000003
        Journeys    Western Ukraine    44444444-0000-0000-0000-000000000001 (Olena)
                    Krakow weekend     44444444-0000-0000-0000-000000000002 (Andrii)
        Trips       Lviv               55555555-0000-0000-0000-000000000001 (booking Pending)
                    Carpathians        55555555-0000-0000-0000-000000000002 (booking Confirmed)
                    Krakow             55555555-0000-0000-0000-000000000003 (no booking)
                    Kyiv (Completed)   55555555-0000-0000-0000-000000000004
        Bookings    66666666-0000-0000-0000-00000000000[1-3]

    Enum values: JourneyStatus Active=0, Passed=1 | TripStatus Active=0, Completed=1
                 TravelStyle Relaxed=0, Balanced=1, Active=2, Adventure=3
                 PersonalityType Introvert=0, Ambivert=1, Extrovert=2 | BudgetLevel Low=0, Medium=1, High=2
                 CompanionGender Any=0, Female=1, Male=2 | NotificationType Info=0, Journey=1, Booking=2
                 BookingStatus Pending=0, Confirmed=1, Cancelled=2, Completed=3
                 TransportType FLIGHT=0, TRAIN=1, BUS=2 | SeatClass Economy=0, Business=1
                 ActivityType ENTERTAINMENT=0, EDUCATION=1, SPORTS=2 | PlaceStatus Free=0
                 FoodOptions None=0, BreakfastOnly=1, HalfBoard=2
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

DECLARE @lviv     uniqueidentifier = '22222222-0000-0000-0000-000000000001';
DECLARE @kyiv     uniqueidentifier = '22222222-0000-0000-0000-000000000002';
DECLARE @yaremche uniqueidentifier = '22222222-0000-0000-0000-000000000003';
DECLARE @krakow   uniqueidentifier = '22222222-0000-0000-0000-000000000004';
DECLARE @lvivOpera    uniqueidentifier = '22222222-0000-0000-0000-000000000005';
DECLARE @lvivCastle   uniqueidentifier = '22222222-0000-0000-0000-000000000006';
DECLARE @lvivLychakiv uniqueidentifier = '22222222-0000-0000-0000-000000000007';
DECLARE @lvivPotocki  uniqueidentifier = '22222222-0000-0000-0000-000000000008';

DECLARE @placeLviv   uniqueidentifier = '33333333-0000-0000-0000-000000000001';
DECLARE @placeChalet uniqueidentifier = '33333333-0000-0000-0000-000000000002';
DECLARE @placeKyiv   uniqueidentifier = '33333333-0000-0000-0000-000000000003';

DECLARE @actCoffee uniqueidentifier = '77777777-0000-0000-0000-000000000001';
DECLARE @actHike   uniqueidentifier = '77777777-0000-0000-0000-000000000002';
DECLARE @actWawel  uniqueidentifier = '77777777-0000-0000-0000-000000000003';

DECLARE @budgetJourney1 uniqueidentifier = '88888888-0000-0000-0000-000000000001';
DECLARE @budgetTrip1    uniqueidentifier = '88888888-0000-0000-0000-000000000002';
DECLARE @budgetTrip2    uniqueidentifier = '88888888-0000-0000-0000-000000000003';
DECLARE @budgetTrip3    uniqueidentifier = '88888888-0000-0000-0000-000000000004';

DECLARE @journeyUkraine uniqueidentifier = '44444444-0000-0000-0000-000000000001';
DECLARE @journeyKrakow  uniqueidentifier = '44444444-0000-0000-0000-000000000002';

DECLARE @tripLviv    uniqueidentifier = '55555555-0000-0000-0000-000000000001';
DECLARE @tripCarpat  uniqueidentifier = '55555555-0000-0000-0000-000000000002';
DECLARE @tripKrakow  uniqueidentifier = '55555555-0000-0000-0000-000000000003';
DECLARE @tripKyiv    uniqueidentifier = '55555555-0000-0000-0000-000000000004';

DECLARE @bookingLviv   uniqueidentifier = '66666666-0000-0000-0000-000000000001';
DECLARE @bookingChalet uniqueidentifier = '66666666-0000-0000-0000-000000000002';
DECLARE @bookingKyiv   uniqueidentifier = '66666666-0000-0000-0000-000000000003';

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

-- Locations (WGS 84 coordinates)
INSERT INTO [Locations] ([Id], [Name], [CountryId], [City], [Street], [ZipCode], [Latitude], [Longitude], [Created], [Updated])
VALUES
    (@lviv,         N'Rynok Square',            @ukraine, N'Lviv',     N'Rynok Square 1',        N'79008',  49.841900, 24.031600, @now, @now),
    (@kyiv,         N'Khreshchatyk',            @ukraine, N'Kyiv',     N'Khreshchatyk St 22',    N'01001',  50.447400, 30.522600, @now, @now),
    (@yaremche,     N'Yaremche centre',         @ukraine, N'Yaremche', N'Svobody St 5',          N'78500',  48.458300, 24.555000, @now, @now),
    (@krakow,       N'Wawel Royal Castle',      @poland,  N'Krakow',   N'Wawel 5',               N'31-001', 50.054000, 19.935400, @now, @now),
    (@lvivOpera,    N'Lviv Opera House',        @ukraine, N'Lviv',     N'Svobody Ave 28',        N'79000',  49.844000, 24.026200, @now, @now),
    (@lvivCastle,   N'High Castle',             @ukraine, N'Lviv',     N'Vysokyi Zamok St',      N'79008',  49.848300, 24.039500, @now, @now),
    (@lvivLychakiv, N'Lychakiv Cemetery',       @ukraine, N'Lviv',     N'Mechnykova St 33',      N'79013',  49.831600, 24.055800, @now, @now),
    (@lvivPotocki,  N'Potocki Palace',          @ukraine, N'Lviv',     N'Kopernyka St 15',       N'79000',  49.836500, 24.022600, @now, @now);

-- Places (owned by the host)
INSERT INTO [Places] ([Id], [Name], [Description], [Type], [Address], [PricePerHour], [Capacity], [IsAvailable], [Status],
                      [AverageRating], [ImageUrls], [Food], [BookingRules], [LocationId], [OwnerId], [Created], [Updated])
VALUES
    (@placeLviv,   N'Old Town Apartments', N'Cozy apartments near Rynok Square', N'Apartment', N'Rynok Square 1, Lviv',
     4.50, 4, 1, 0, 4.7, N'["https://example.com/img/lviv-1.jpg"]', 0, N'Check-in after 14:00', @lviv, @iryna, @now, @now),
    (@placeChalet, N'Carpathian Chalet', N'Wooden chalet with mountain view', N'Chalet', N'Svobody St 5, Yaremche',
     6.25, 6, 1, 0, 4.9, N'["https://example.com/img/chalet-1.jpg","https://example.com/img/chalet-2.jpg"]', 2,
     N'No smoking', @yaremche, @iryna, @now, @now),
    (@placeKyiv,   N'Kyiv Riverside Hotel', N'Business hotel on the Dnipro embankment', N'Hotel', N'Khreshchatyk St 22, Kyiv',
     8.00, 2, 1, 0, 4.3, N'[]', 1, NULL, @kyiv, @iryna, @now, @now);

-- Activities
INSERT INTO [Activities] ([Id], [Name], [Type], [Description], [Price], [Status], [Rating], [ImageUrl], [VideoUrl],
                          [Period_Start], [Period_End], [LocationId], [Created], [Updated])
VALUES
    (@actCoffee, N'Lviv coffee mine tour', 0, N'Guided tour with coffee tasting', 12.00, 0, 4.8,
     N'https://example.com/img/coffee.jpg', NULL, DATEADD(hour, 30*24 + 15, @today), DATEADD(hour, 30*24 + 17, @today), @lviv, @now, @now),
    (@actHike,   N'Hoverla hike', 2, N'One day hike to the highest peak of Ukraine', 35.00, 0, 4.9,
     NULL, NULL, DATEADD(hour, 36*24 + 7, @today), DATEADD(hour, 36*24 + 19, @today), @yaremche, @now, @now),
    (@actWawel,  N'Wawel castle tour', 1, N'Royal castle and cathedral', 20.00, 0, 4.6,
     NULL, NULL, DATEADD(hour, 61*24 + 10, @today), DATEADD(hour, 61*24 + 13, @today), @krakow, @now, @now);

-- Reviews (each review targets a place or an activity)
INSERT INTO [Reviews] ([Id], [UserId], [PlaceId], [ActivityId], [Rating], [Comment], [Title], [IsVisible], [LikesCount], [Created], [Updated])
VALUES
    ('aaaaaaaa-0000-0000-0000-000000000001', @andrii, @placeLviv,   NULL,      5, N'Great location, very clean', N'Perfect stay',  1, 3, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000002', @andrii, @placeKyiv,   NULL,      4, N'Good for business trips',    N'Solid hotel',   1, 1, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000003', @olena,  NULL,         @actCoffee, 5, N'Loved the coffee tasting',  N'Must visit',    1, 7, @now, @now),
    ('aaaaaaaa-0000-0000-0000-000000000004', @andrii, NULL,         @actHike,  4, N'Hard but beautiful',         N'Worth it',      1, 2, @now, @now);

-- Budgets (limit / calculated total of trips and journeys)
INSERT INTO [Budgets] ([Id], [Budget], [Total], [CurrencyCode], [Created], [Updated])
VALUES
    (@budgetJourney1, 3000.00, 1370.00, N'USD', @now, @now),
    (@budgetTrip1,    1200.00,  450.00, N'USD', @now, @now),
    (@budgetTrip2,    1500.00,  920.00, N'USD', @now, @now),
    (@budgetTrip3,     800.00,  120.00, N'USD', @now, @now);

-- Journeys
INSERT INTO [Journeys] ([Id], [Status], [Title], [Description], [Period_Start], [Period_End], [Members], [BudgetId], [UserId], [Created], [Updated])
VALUES
    (@journeyUkraine, 0, N'Western Ukraine tour', N'Lviv old town and Carpathian mountains',
     DATEADD(day, 30, @today), DATEADD(day, 40, @today), N'["Olena","Taras"]', @budgetJourney1, @olena, @now, @now),
    (@journeyKrakow,  0, N'Krakow weekend', N'Short city break',
     DATEADD(day, 60, @today), DATEADD(day, 63, @today), N'["Andrii"]', NULL, @andrii, @now, @now);

-- Bookings (trips reference their booking)
INSERT INTO [Bookings] ([Id], [UserId], [JourneyId], [PropertyId], [RoomId], [Period_Start], [Period_End], [TotalPrice], [Currency],
                        [IsPaid], [PaymentMethod], [Adults], [Children], [Status], [IsFrozen], [CheckInDate], [CheckOutDate], [BudgetId],
                        [Created], [Updated])
VALUES
    (@bookingLviv,   @olena,  @journeyUkraine, @placeLviv,   NULL,
     DATEADD(day, 30, @today), DATEADD(day, 34, @today), 400.00, N'USD', 0, NULL,   2, 0, 0, 0,
     DATEADD(day, 30, @today), DATEADD(day, 34, @today), NULL, @now, @now),
    (@bookingChalet, @olena,  @journeyUkraine, @placeChalet, NULL,
     DATEADD(day, 34, @today), DATEADD(day, 40, @today), 900.00, N'USD', 1, N'card', 2, 1, 1, 0,
     DATEADD(day, 34, @today), DATEADD(day, 40, @today), NULL, @now, @now),
    (@bookingKyiv,   @andrii, NULL,            @placeKyiv,   NULL,
     DATEADD(day, -20, @today), DATEADD(day, -17, @today), 240.00, N'USD', 1, N'card', 1, 0, 3, 1,
     DATEADD(day, -20, @today), DATEADD(day, -17, @today), NULL, @now, @now);

-- Trips
INSERT INTO [Trips] ([Id], [Name], [Rating], [Status], [Period_Start], [Period_End], [UserId], [JourneyId], [BudgetId], [BookingId], [Created], [Updated])
VALUES
    (@tripLviv,   N'Lviv',              0,   0, DATEADD(day, 30, @today), DATEADD(day, 34, @today),   @olena,  @journeyUkraine, @budgetTrip1, @bookingLviv,   @now, @now),
    (@tripCarpat, N'Carpathians',       0,   0, DATEADD(day, 34, @today), DATEADD(day, 40, @today),   @olena,  @journeyUkraine, @budgetTrip2, @bookingChalet, @now, @now),
    (@tripKrakow, N'Krakow',            0,   0, DATEADD(day, 60, @today), DATEADD(day, 63, @today),   @andrii, @journeyKrakow,  @budgetTrip3, NULL,           @now, @now),
    (@tripKyiv,   N'Kyiv business trip', 8.5, 1, DATEADD(day, -20, @today), DATEADD(day, -17, @today), @andrii, NULL,            NULL,         @bookingKyiv,   @now, @now);

-- Transports (Duration is stored in ticks: 1 hour = 36000000000)
INSERT INTO [Transports] ([Id], [TripId], [JourneyId], [Type], [Company], [Price], [Period_Start], [Period_End], [SeatClass], [SeatCount],
                          [Duration], [Created], [Updated])
VALUES
    ('bbbbbbbb-0000-0000-0000-000000000001', @tripLviv,   @journeyUkraine, 1, N'Ukrzaliznytsia', 50.00,
     DATEADD(hour, 30*24 + 8, @today), DATEADD(hour, 30*24 + 13, @today), 0, 2, 5 * CAST(36000000000 AS bigint), @now, @now),
    ('bbbbbbbb-0000-0000-0000-000000000002', @tripCarpat, @journeyUkraine, 2, N'Carpathian Bus', 20.00,
     DATEADD(hour, 34*24 + 9, @today), DATEADD(hour, 34*24 + 12, @today), 0, 2, 3 * CAST(36000000000 AS bigint), @now, @now),
    ('bbbbbbbb-0000-0000-0000-000000000003', @tripKrakow, @journeyKrakow,  0, N'LOT Polish Airlines', 120.00,
     DATEADD(hour, 60*24 + 7, @today), DATEADD(minute, 60*24*60 + 7*60 + 75, @today), 0, 1, CAST(45000000000 AS bigint), @now, @now),
    ('bbbbbbbb-0000-0000-0000-000000000004', @tripKyiv,   NULL,            1, N'Intercity+', 35.00,
     DATEADD(hour, -20*24 + 6, @today), DATEADD(hour, -20*24 + 11, @today), 1, 1, 5 * CAST(36000000000 AS bigint), @now, @now);

-- Travel profiles
INSERT INTO [UserInfos] ([Id], [UserId], [BirthDate], [TravelStyle], [PersonalityType], [BudgetLevel], [CompanionGender],
                         [LookingFor], [Languages], [Interests], [Created], [Updated])
VALUES
    ('99999999-0000-0000-0000-000000000001', @olena,  '1997-05-14', 2, 2, 1, 0,
     N'Travel companions for hiking weekends', N'["uk","en"]', N'[2,3]', @now, @now),
    ('99999999-0000-0000-0000-000000000002', @andrii, '1992-11-02', 0, 0, 2, 0,
     N'Calm city breaks with museums', N'["uk","en","pl"]', N'[1,0]', @now, @now);

INSERT INTO [UserInfoChosenActivities] ([UserInfoId], [ActivityId])
VALUES
    ('99999999-0000-0000-0000-000000000001', @actHike),
    ('99999999-0000-0000-0000-000000000001', @actCoffee),
    ('99999999-0000-0000-0000-000000000002', @actWawel);

INSERT INTO [UserInfoChosenTrips] ([UserInfoId], [TripId])
VALUES
    ('99999999-0000-0000-0000-000000000001', @tripCarpat),
    ('99999999-0000-0000-0000-000000000002', @tripLviv);

INSERT INTO [UserInfoPreferredCountries] ([UserInfoId], [CountryId])
VALUES
    ('99999999-0000-0000-0000-000000000001', @ukraine),
    ('99999999-0000-0000-0000-000000000002', @poland),
    ('99999999-0000-0000-0000-000000000002', @ukraine);

-- Notifications
INSERT INTO [Notifications] ([Id], [UserId], [Type], [Title], [Message], [IsRead], [ReadAt], [Created], [Updated])
VALUES
    ('dddddddd-0000-0000-0000-000000000001', @olena,  1, N'Journey created', N'Western Ukraine tour is ready for planning.', 1, @now, @now, @now),
    ('dddddddd-0000-0000-0000-000000000002', @olena,  2, N'Booking confirmed', N'Carpathian Chalet booking was paid.',        0, NULL, @now, @now),
    ('dddddddd-0000-0000-0000-000000000003', @andrii, 0, N'Welcome to TravellerAI', N'Fill in your travel profile.',           0, NULL, @now, @now);

COMMIT TRANSACTION;

PRINT 'Test data inserted';
