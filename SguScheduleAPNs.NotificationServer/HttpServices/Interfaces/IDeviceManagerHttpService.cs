using Core.Entities;

namespace SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;

public interface IDeviceManagerHttpService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken token);
}