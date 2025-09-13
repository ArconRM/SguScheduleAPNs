using Core.Entities;
using SguScheduleAPNs.DevicesManager.DTO;
using SguScheduleAPNs.DevicesManager.Repository.Interfaces;
using SguScheduleAPNs.DevicesManager.Service.Interfaces;

namespace SguScheduleAPNs.DevicesManager.Service;

public class DeviceManagerService: IDeviceManagerService
{
    private readonly IDeviceRepository _repository;

    public DeviceManagerService(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken token)
    {
        return await _repository.GetAllAsync(token);
    }
    
    public async Task<Guid> RegisterDeviceAsync(DeviceRegisterRequest request, CancellationToken token)
    {
        var device = new Device
        {
            ApnsToken = request.ApnsToken,
            Model = request.Model,
            SystemVersion = request.SystemVersion,
            FavouriteGroupDepartment = request.FavouriteGroupDepartment,
            FavouriteGroupNumber = request.FavouriteGroupNumber
        };
        
        var createdDevice = await _repository.CreateAsync(device, token);
        return createdDevice.Uuid;
    }
    
    public async Task UpdateFavouriteGroupAsync(FavouriteGroupUpdateRequest request, CancellationToken token)
    {
        var device = await _repository.GetDeviceByTokenAsync(request.apnsToken, token);
        device.FavouriteGroupDepartment = request.FavouriteGroupDepartment;
        device.FavouriteGroupNumber = request.FavouriteGroupNumber;
        await _repository.UpdateAsync(device, token);
    }

    public async Task UnregisterDeviceAsync(string apnsToken, CancellationToken token)
    {
        var device = await _repository.GetDeviceByTokenAsync(apnsToken, token);
        await _repository.DeleteAsync(device.Uuid, token);
    }
}