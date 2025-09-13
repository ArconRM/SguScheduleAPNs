namespace SguScheduleAPNs.NotificationServer.Options;

public class ApnsServiceOptions
{
    public string BundleId { get; set; }
    public string CertificatePath { get; set; }

    public string KeyId { get; set; }

    public string TeamId { get; set; }
}