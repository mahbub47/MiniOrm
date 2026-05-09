using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MiniOrm.Data.Metadata;

/// <summary>
/// This class represents metadata information for a property of an entity. It includes details such as the property's name,
/// </summary>
public class PropertyMetadata
{
    /// <summary>
    /// Name of the property. This is typically the name of the property in the entity class, 
    /// and it can be used for mapping to database columns or for serialization purposes.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Type of the property in the CLR (Common Language Runtime). 
    /// This information is crucial for understanding the data type of the property,
    /// </summary>
    [JsonIgnore]
    public Type? ClrType { get; set; }

    /// <summary>
    /// Database type of the property. This is the type that corresponds 
    /// to the database column type, which may differ from the CLR type.
    /// </summary>
    public string? DatabaseType { get; set; }

    /// <summary>
    /// IsPrimaryKey indicates whether the property is a primary key in the database. 
    /// This information is essential for identifying the unique identifier of an entity,
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// IsNullable indicates whether the property can accept null values. 
    /// This information is important for understanding the constraints of the property in the database schema,
    /// </summary>
<<<<<<< HEAD
    public string? Nullable { get; set; }
=======
    public bool IsNullable { get; set; }
>>>>>>> 29067a9aad8df6c51a3f6159891f75b78f3dba8d
}
