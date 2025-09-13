using Core.BaseEntities.Interfaces;

namespace Core.Entities;

public class Device: IEntity
{
    public Guid Uuid { get; set; }
    
    public string ApnsToken { get; set; }

    public string Model { get; set; }

    public string SystemVersion { get; set; }

    public string FavouriteGroupDepartment { get; set; }

    public string FavouriteGroupNumber { get; set; }
}