namespace SguScheduleAPNs.DevicesManager.DTO;

public class DeviceRegisterRequest
{
    public string apnsToken { get; set; }

    public string Model { get; set; }

    public string SystemVersion { get; set; }

    public string FavouriteGroupDepartment { get; set; }

    public string FavouriteGroupNumber { get; set; }
}