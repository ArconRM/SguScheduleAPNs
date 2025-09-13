namespace SguScheduleAPNs.NotificationServer.Entities;

public class Schedule
{
    public string GroupKey { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<Lesson> Lessons { get; set; }
}