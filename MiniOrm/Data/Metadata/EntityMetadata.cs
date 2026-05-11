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
    // This represents the name of the database table in the current dbcontext.
    public string? Name { get; set; }

    // This represents the CLR type of the entity, which is used for mapping between the database and the application.
    [JsonConverter(typeof(TypeConverter))]
    public Type? EntityType { get; set; }

    // This represents the list of columns in the database table in the current dbcontext.
    public List<PropertyMetadata>? Properties { get; set; } = new();
}
