using Autofac;
using Autofac.Extensions.DependencyInjection;
using Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;
using TravellerAI.Bootstrap;
using TravellerAI.Infrastructure.Db.Mssql.Context;
using TravellerAI.Middleware;
using TravellerAI.Settings;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterApplicationServices();
builder.Services.RegisterDatabase(builder.Configuration);
builder.Services.AddControllersWithViews(options =>
    {
        // request validation is done by FluentValidation validators in the MediatR pipeline
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // models reference each other (trip -> user -> journeys -> trips)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new ApplicationBootstrapperModule(new AppSettings()));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    var appVer = "** Ver UNKNOWN **";
    var exeLoc = Assembly.GetExecutingAssembly().Location;
    var lastUpdate = $"Deployment date {File.GetCreationTime(exeLoc)}";

    var verFilePath = Path.Combine(AppContext.BaseDirectory, ".version");

    if (File.Exists(verFilePath))
    {
        appVer = File.ReadAllText(verFilePath);
    }

    swaggerGenOptions.SwaggerDoc("v1", new() { Description = lastUpdate, Title = "TravellerAI", Version = appVer });

    // XML docs of the solution projects (GenerateDocumentationFile), controller summaries are used for groups
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "TravellerAI*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => swaggerGenOptions.IncludeXmlComments(xmlFile, includeControllerXmlComments: true));
});


var app = builder.Build();

// Development: creates / migrates the database and fills it with test data (see appsettings.Development.json)
if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    await DatabaseInitializer.InitializeAsync(app.Services, app.Configuration.GetValue<bool>("Database:SeedTestData"));
}

// maps exceptions to HTTP status codes, see GlobalExceptionHandler
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // swagger.json: /swagger/v1/swagger.json, UI: /swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TravellerAI API v1");
        options.DocumentTitle = "TravellerAI API";
        options.DisplayRequestDuration();
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();