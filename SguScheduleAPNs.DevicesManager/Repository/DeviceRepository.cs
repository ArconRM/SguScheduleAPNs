using Core.BaseEntities;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using SguScheduleAPNs.DevicesManager.Repository.Interfaces;

namespace SguScheduleAPNs.DevicesManager.Repository;

public class DeviceRepository: BaseRepository<Device>, IDeviceRepository
{
    private readonly DevicesManagerDbContext _context;

    public DeviceRepository(DevicesManagerDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Device?> GetDeviceByTokenAsync(string apnsToken, CancellationToken token)
    {
        var set = _context.Set<Device>();
        return await set.AsNoTracking().Where(d => d.ApnsToken == apnsToken).FirstOrDefaultAsync(token);
    }
}