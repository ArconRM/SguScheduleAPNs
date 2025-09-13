using DotNetEnv;
using SguScheduleAPNs.NotificationServer.HttpServices;
using SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;
using SguScheduleAPNs.NotificationServer.Options;
using SguScheduleAPNs.NotificationServer.Service;
using SguScheduleAPNs.NotificationServer.Service.Interfaces;

// For Apns Service
Env.Load();

var builder = WebApplication.CreateBuilder(args);

var deviceManagerAddress = Environment.GetEnvironmentVariable("DEVICE_MANAGER_BASE_ADDRESS");
var parsingServerAddress = builder.Configuration.GetSection("ParsingServiceOptions")["ServerAddress"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ParsingServiceOptions>(
    builder.Configuration.GetSection("ParsingServiceOptions"));

builder.Services.Configure<ApnsServiceOptions> ( options =>
{
    options.BundleId = Environment.GetEnvironmentVariable("APNS_BUNDLE_ID");
    options.CertificatePath = Environment.GetEnvironmentVariable("APNS_KEY_PATH");
    options.KeyId = Environment.GetEnvironmentVariable("APNS_KEY_ID");
    options.TeamId = Environment.GetEnvironmentVariable("APNS_TEAM_ID");
});

builder.Services.Configure<SchedulePersistenceServiceOptions>(
    builder.Configuration.GetSection("SchedulePersistenceServiceOptions"));

builder.Services.AddHttpClient<IDeviceManagerHttpService, DeviceManagerHttpService>(client =>
{
    client.BaseAddress = new Uri(deviceManagerAddress);
    client.Timeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddHttpClient<IParsingHttpService, ParsingHttpService>(client =>
{
    client.BaseAddress = new Uri(parsingServerAddress);
    client.Timeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddScoped<ISchedulePersistenceService, SchedulePersistenceService>();
builder.Services.AddScoped<IApnsService, ApnsService>();
        
builder.Services.AddHostedService<NotificationBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
