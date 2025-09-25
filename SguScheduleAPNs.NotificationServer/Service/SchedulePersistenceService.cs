using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.Extensions.Options;
using SguScheduleAPNs.NotificationServer.Entities;
using SguScheduleAPNs.NotificationServer.Options;
using SguScheduleAPNs.NotificationServer.Service.Interfaces;

namespace SguScheduleAPNs.NotificationServer.Service;

public class SchedulePersistenceService : ISchedulePersistenceService
{
    private readonly string _dataPath;
    private readonly string _dataHistoryPath;
    private readonly ILogger<SchedulePersistenceService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public SchedulePersistenceService(
        IOptions<SchedulePersistenceServiceOptions> options,
        ILogger<SchedulePersistenceService> logger)
    {
        _dataPath = options.Value.DataPath;
        _dataHistoryPath = Path.Combine(_dataPath, "History");
        _logger = logger;
        Directory.CreateDirectory(_dataPath);
        Directory.CreateDirectory(_dataHistoryPath);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    public async Task<List<Lesson>?> LoadScheduleAsync(
        string groupKey,
        CancellationToken token)
    {
        var file = GetFilePath(groupKey);
        if (!File.Exists(file))
            return null;

        try
        {
            var json = await File.ReadAllTextAsync(file, token);
            return JsonSerializer.Deserialize<List<Lesson>>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load schedule for group {Group}", groupKey);
            return null;
        }
    }

    public async Task SaveScheduleAsync(
        string groupKey,
        List<Lesson> lessons,
        CancellationToken token)
    {
        var file = GetFilePath(groupKey);
        try
        {
            var json = JsonSerializer.Serialize(lessons, _jsonOptions);
            await File.WriteAllTextAsync(file, json, token);
            _logger.LogInformation("Saved schedule for group {Group}", groupKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save schedule for group {Group}", groupKey);
        }
    }

    public async Task SaveScheduleWithArchivingOldVersionAsync(
        string groupKey,
        List<Lesson> lessons,
        CancellationToken token)
    {
        var file = GetFilePath(groupKey);
        try
        {
            if (File.Exists(file))
            {
                var oldJson = await File.ReadAllTextAsync(file, token);
                if (!string.IsNullOrEmpty(oldJson))
                {
                    var historyFile = GetFilePath($"{groupKey}_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}", isHistory: true);
                    await File.WriteAllTextAsync(historyFile, oldJson, token);
                }
            }

            var json = JsonSerializer.Serialize(lessons, _jsonOptions);
            await File.WriteAllTextAsync(file, json, token);
            _logger.LogInformation("Saved schedule for group {Group}", groupKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save schedule for group {Group}", groupKey);
        }
    }

    public async Task<bool> HasScheduleChangedAsync(
        string groupKey,
        List<Lesson> newLessons,
        CancellationToken token)
    {
        var oldLessons = await LoadScheduleAsync(groupKey, token);
        if (oldLessons is null)
            return true;

        if (oldLessons.Count != newLessons.Count)
            return true;

        var changed = !oldLessons
            .OrderBy(l => l.GetHashCode())
            .SequenceEqual(newLessons.OrderBy(l => l.GetHashCode()));

        return changed;
    }

    private string GetFilePath(string fileName, bool isHistory = false)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c, '_');

        return Path.Combine(isHistory ? _dataHistoryPath : _dataPath, $"{fileName}.json");
    }
}