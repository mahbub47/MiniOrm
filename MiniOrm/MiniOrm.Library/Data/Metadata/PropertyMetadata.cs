using MiniOrm.MiniOrm.Library.Helper;
using System.Text.Json.Serialization;

namespace MiniOrm.MiniOrm.Library.Data.Metadata;

/// <summary>
/// This class represents metadata information for a property of an entity. It includes details such as the property's name,
/// </summary>
public class PropertyMetadata
{
    // Represents the name of a column in a database table.
    public string? Name { get; set; }

    // This property represents the CLR type of a database column
    [JsonConverter(typeof(TypeConverter))]
    public Type? ClrType { get; set; }

    // This property represents the database type of a column, such as "TEXT" or "INT".
    public string? DatabaseType { get; set; }

    // IsPrimaryKey indicates whether the property is a primary key in the database schema.
    public bool IsPrimaryKey { get; set; } = false;


    // It represents the to column is NULL or NOT NULL
    public string? Nullable { get; set; }
}
