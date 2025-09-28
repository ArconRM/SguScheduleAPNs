using Core.Entities;
using Microsoft.Extensions.Options;
using SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;
using SguScheduleAPNs.NotificationServer.Options;
using SguScheduleAPNs.NotificationServer.Service.Interfaces;

namespace SguScheduleAPNs.NotificationServer.Service;

public class NotificationBackgroundService : BackgroundService
{
    private readonly string _dataPath;
    private readonly Random _random;
    private readonly ILogger<NotificationBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDeviceManagerHttpService _deviceManagerHttpService;
    private readonly IParsingHttpService _parsingHttpService;

    public NotificationBackgroundService(
        IOptions<SchedulePersistenceServiceOptions> options,
        ILogger<NotificationBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IDeviceManagerHttpService deviceManagerHttpService,
        IParsingHttpService parsingHttpService)
    {
        _dataPath = options.Value.DataPath;
        _random = new Random();
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _deviceManagerHttpService = deviceManagerHttpService;
        _parsingHttpService = parsingHttpService;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (IsBusinessHours())
            {
                await ProcessNotifications(token);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(_random.Next(15, 35)), token);
        }
    }

    private async Task ProcessNotifications(CancellationToken token)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider;
        var scheduleService = provider.GetRequiredService<ISchedulePersistenceService>();
        var apnsService = provider.GetRequiredService<IApnsService>();
        
        Dictionary<string, List<Device>> devicesGroupMap;
        try
        {
            var devices = await _deviceManagerHttpService.GetAllDevicesAsync(token);
            devicesGroupMap = devices
                .GroupBy(d => $"{d.FavouriteGroupDepartment}/do/{d.FavouriteGroupNumber}")
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch device tokens.");
            return;
        }

        foreach (var group in devicesGroupMap)
        {
            try
            {
                var parsedLessons = await _parsingHttpService.ParseLessonsInGroupAsync(group.Key, token);
                var parsedLessonsList = parsedLessons.ToList();

                if (await scheduleService.LoadScheduleAsync(group.Key, token) is null)
                {
                    await scheduleService.SaveScheduleAsync(group.Key, parsedLessonsList, token);
                    return;
                }

                var hasChanged = await scheduleService.HasScheduleChangedAsync(group.Key, parsedLessonsList, token);
                if (!hasChanged)
                {
                    _logger.LogInformation("No schedule change for {Group}", group.Key);
                    continue;
                }

                foreach (var device in group.Value)
                    await apnsService.SendNotificationAsync(
                        "Расписание СГУ",
                        "Расписание избранной группы обновилось",
                        device.ApnsToken);

                _logger.LogInformation("Schedule changed for {Group}, notified {Count} devices.",
                    group.Key, group.Value.Count);

                await scheduleService.SaveScheduleWithArchivingOldVersionAsync(group.Key, parsedLessonsList, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process group {Group}", group.Key);
            }
        }
    }
    
    private bool IsBusinessHours()
    {
        var now = DateTime.Now;
        return now.Hour >= 7 && now.Hour < 23;
    }
}
