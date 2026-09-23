namespace TravellerAI.Domain.Entities;

public class CountryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    /// <summary>ISO 3166-1 alpha-2 code, e.g. UA.</summary>
    public string Code { get; set; } = string.Empty;

    public virtual ICollection<LocationEntity> Locations { get; set; } = new List<LocationEntity>();
}
