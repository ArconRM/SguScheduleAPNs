using System.Security.Cryptography.X509Certificates;
using dotAPNS;
using Microsoft.Extensions.Options;
using SguScheduleAPNs.NotificationServer.Options;
using SguScheduleAPNs.NotificationServer.Service.Interfaces;

namespace SguScheduleAPNs.NotificationServer.Service;

public class ApnsService : IApnsService
{
    private readonly string _bundleId;
    private readonly string _certificatePath;
    private readonly string _keyId;
    private readonly string _teamId;
    private readonly ILogger<ApnsService> _logger;

    public ApnsService(IOptions<ApnsServiceOptions> options, ILogger<ApnsService> logger)
    {
        _bundleId = options.Value.BundleId;
        _certificatePath = options.Value.CertificatePath;
        _keyId = options.Value.KeyId;
        _teamId = options.Value.TeamId;
        _logger = logger;
    }

    public async Task<bool> SendNotificationAsync(string title, string body, string token)
    {
        var options = new ApnsJwtOptions
        {
            BundleId = _bundleId,
            CertFilePath = _certificatePath,
            KeyId = _keyId,
            TeamId = _teamId,
        };

        var apns = ApnsClient.CreateUsingJwt(new HttpClient(), options);

        var push = new ApplePush(ApplePushType.Alert)
            .AddAlert(title, body)
            .AddToken(token);
            
            // .SendToDevelopmentServer();

        try
        {
            var response = await apns.SendAsync(push);
            if (response.IsSuccessful)
            {
                _logger.LogInformation($"Successfully sent notification to {token}");
                return true;
            }

            switch (response.Reason)
            {
                case ApnsResponseReason.BadCertificateEnvironment:
                    // The client certificate is for the wrong environment.
                    // TODO: retry on another environment
                    break;
                // TODO: process other reasons we might be interested in
                default:
                    throw new ArgumentOutOfRangeException(nameof(response.Reason), response.Reason, null);
            }

            _logger.LogError("Failed to send a push, APNs reported an error: " + response.ReasonString);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Failed to send a push: HTTP request timed out.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Failed to send a push. HTTP request failed: " + ex);
        }
        catch (ApnsCertificateExpiredException)
        {
            _logger.LogError(
                "APNs certificate has expired. No more push notifications can be sent using it until it is replaced with a new one.");
        }

        return false;
    }
}