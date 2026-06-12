using Autofac;
using Autofac.Extensions.DependencyInjection;
using Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;
using TravellerAI.Bootstrap;
using TravellerAI.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterApplicationServices();
builder.Services.AddControllersWithViews();

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

    var dir = Path.GetDirectoryName(exeLoc);
    // ReSharper disable once AssignNullToNotNullAttribute
    var verFilePath = Path.Combine(dir, ".version");

    if (File.Exists(verFilePath))
    {
        appVer = File.ReadAllText(verFilePath);
    }

    swaggerGenOptions.SwaggerDoc("v1", new() { Description = lastUpdate, Title = "TravellerAI", Version = appVer });


    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => swaggerGenOptions.IncludeXmlComments(xmlFile));

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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