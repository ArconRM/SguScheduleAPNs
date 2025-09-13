using Microsoft.AspNetCore.Mvc;
using SguScheduleAPNs.NotificationServer.Service.Interfaces;

namespace SguScheduleAPNs.NotificationServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationServerController : ControllerBase
{
    private readonly ILogger<NotificationServerController> _logger;
    private readonly IApnsService _apnsService;

    public NotificationServerController(
        ILogger<NotificationServerController> logger,
        IApnsService apnsService)
    {
        _logger = logger;
        _apnsService = apnsService;
    }

    [HttpPost(nameof(SendTestNotification))]
    public async Task<IActionResult> SendTestNotification(
        string apnsToken,
        string title,
        string body,
        CancellationToken token)
    {
        try
        {
            await _apnsService.SendNotificationAsync(title, body, apnsToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}