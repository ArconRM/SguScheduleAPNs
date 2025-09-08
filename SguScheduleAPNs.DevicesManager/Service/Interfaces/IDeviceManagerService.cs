using SguScheduleAPNs.DevicesManager.DTO;
using SguScheduleAPNs.DevicesManager.Entities;

namespace SguScheduleAPNs.DevicesManager.Service.Interfaces;

public interface IDeviceManagerService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken token);

    Task<Guid> RegisterDeviceAsync(DeviceRegisterRequest request, CancellationToken token);
    
    Task UpdateFavouriteGroupAsync(FavouriteGroupUpdateRequest request, CancellationToken token);
}