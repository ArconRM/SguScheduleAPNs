using SguScheduleAPNs.NotificationServer.Entities;

namespace SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;

public interface IParsingHttpService
{
    Task<IEnumerable<Lesson>> ParseLessonsInGroupAsync(string groupNumber, CancellationToken token);
}