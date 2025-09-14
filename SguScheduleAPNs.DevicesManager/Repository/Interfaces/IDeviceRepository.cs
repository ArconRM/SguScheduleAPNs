using Core.BaseEntities.Interfaces;
using Core.Entities;

namespace SguScheduleAPNs.DevicesManager.Repository.Interfaces;

public interface IDeviceRepository : IRepository<Device>
{
    Task<Device?> GetDeviceByTokenAsync(string apnsToken, CancellationToken token);
}