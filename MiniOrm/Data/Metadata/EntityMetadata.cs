using MiniOrm.Helper;
using System.Text.Json.Serialization;

namespace MiniOrm.Data.Metadata;

/// <summary>
/// Represents metadata information for an entity, including its name and associated properties.
/// </summary>
/// <remarks>Use this class to describe the structure of an entity at runtime, such as for serialization, mapping,
/// or schema inspection scenarios. The metadata includes the entity's name and a collection of property metadata
/// objects that describe each property of the entity.</remarks>
public class EntityMetadata
{
    public string? Name { get; set; }

    [JsonConverter(typeof(TypeConverter))]
    public Type? EntityType { get; set; }

    /// <summary>
    /// Gets or sets the collection of property metadata associated with the current object.
    /// </summary>
    public List<PropertyMetadata>? Properties { get; set; } = new();
}
