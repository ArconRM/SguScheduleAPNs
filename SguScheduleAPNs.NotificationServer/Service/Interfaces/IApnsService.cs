namespace SguScheduleAPNs.NotificationServer.Service.Interfaces;

public interface IApnsService
{
    Task<bool> SendNotificationAsync(string title, string body, string token);
}