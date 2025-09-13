using SguScheduleAPNs.NotificationServer.Entities;

namespace SguScheduleAPNs.NotificationServer.Service.Interfaces;

public interface ISchedulePersistenceService
{
    Task<List<Lesson>?> LoadScheduleAsync(string groupKey, CancellationToken token);
    Task SaveScheduleAsync(string groupKey, List<Lesson> lessons, CancellationToken token);
    Task<bool> HasScheduleChangedAsync(string groupKey, List<Lesson> newLessons, CancellationToken token);
}