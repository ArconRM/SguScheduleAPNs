using Microsoft.AspNetCore.Mvc;
using SguScheduleAPNs.DevicesManager.DTO;
using SguScheduleAPNs.DevicesManager.Service.Interfaces;

namespace SguScheduleAPNs.DevicesManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceManagerController : ControllerBase
{
    private readonly IDeviceManagerService  _deviceManagerService;
    private readonly ILogger<DeviceManagerController> _logger;

    public DeviceManagerController(
        IDeviceManagerService deviceManagerService,
        ILogger<DeviceManagerController> logger)
    {
        _deviceManagerService = deviceManagerService;
        _logger = logger;
    }

    [HttpGet(nameof(GetAllDevices))]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _deviceManagerService.GetAllDevicesAsync(cancellationToken);
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost(nameof(RegisterDevice))]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceRegisterRequest request, CancellationToken token)
    {
        try
        {
            var registeredDeviceGuid = await _deviceManagerService.RegisterDeviceAsync(request, token);
            _logger.LogInformation($"Registered new device with guid: {registeredDeviceGuid}");
            return Ok(registeredDeviceGuid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPost(nameof(UpdateFavouriteGroup))]
    public async Task<IActionResult> UpdateFavouriteGroup([FromBody] FavouriteGroupUpdateRequest request, CancellationToken token)
    {
        try
        {
            await _deviceManagerService.UpdateFavouriteGroupAsync(request, token);
            _logger.LogInformation($"Updated favourite group on device with token: {request.ApnsToken}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost(nameof(UnregisterDevice))]
    public async Task<IActionResult> UnregisterDevice(string apnsToken, CancellationToken token)
    {
        try
        {
            await _deviceManagerService.UnregisterDeviceAsync(apnsToken, token);
            _logger.LogInformation($"Unregistered device with token: {apnsToken}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}