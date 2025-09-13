using Core.Entities;
using SguScheduleAPNs.DevicesManager.DTO;

namespace SguScheduleAPNs.DevicesManager.Service.Interfaces;

public interface IDeviceManagerService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken token);

    Task<Guid> RegisterDeviceAsync(DeviceRegisterRequest request, CancellationToken token);
    
    Task UpdateFavouriteGroupAsync(FavouriteGroupUpdateRequest request, CancellationToken token);

    Task UnregisterDeviceAsync(string apnsToken, CancellationToken token);
}