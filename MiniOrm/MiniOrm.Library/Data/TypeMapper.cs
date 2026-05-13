using System.Reflection;

namespace MiniOrm.MiniOrm.Library.Data;

/// <summary>
/// This class is responsible for mapping CLR types to corresponding database types. 
/// It provides a method to map a given CLR type to a database type, which includes the database type name and whether the type is nullable. 
/// This mapping is essential for generating SQL commands and defining database schemas based on the properties of entity classes in the application.
/// </summary>
public class TypeMapper
{
    /// <summary>
    /// This method takes a PropertyInfo object representing a property of an entity class and maps its CLR type to a corresponding database type.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public DataBaseType Map(PropertyInfo property)
    {
        Type clrType = property.PropertyType;

        //Get the Core Type (e.g., int from int?)
        Type coreType = Nullable.GetUnderlyingType(clrType) ?? clrType;

        //Map the Core Type to SQL
        string sqlTypeName = coreType switch
        {
            var t when t == typeof(int) => "INT",
            var t when t == typeof(long) => "BIGINT",
            var t when t == typeof(float) => "REAL",
            var t when t == typeof(double) => "DOUBLE PRECISION",
            var t when t == typeof(decimal) => "NUMERIC",
            var t when t == typeof(bool) => "BOOLEAN",
            var t when t == typeof(DateTime) => "TIMESTAMP",
            var t when t == typeof(Guid) => "UUID",
            var t when t == typeof(string) => "TEXT",
            _ => throw new ArgumentException($"Type {clrType.Name} is not supported.")
        };

        //Nullability Check
        bool isNullable = IsPropertyNullable(property);
        string nullability = isNullable ? "NULL" : "NOT NULL";

        return new DataBaseType(sqlTypeName, nullability);
    }

    /// <summary>
    /// This method checks if a given property is nullable. It handles both value types (like int, DateTime) and reference types (like string, classes).
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private bool IsPropertyNullable(PropertyInfo property)
    {
        // For Value Types (int, DateTime, etc.)
        if (property.PropertyType.IsValueType)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) != null;
        }

        // For Reference Types (string, classes)
        var context = new NullabilityInfoContext();
        var info = context.Create(property);
        return info.WriteState == NullabilityState.Nullable;
    }
}

/// <summary>
/// This class represents the database type information for a given CLR type. 
/// It includes the database type name and a flag indicating whether the type is nullable.
/// </summary>
public class DataBaseType
{
    public string? DatabaseType { get; set; }
    public string? Nullable { get; set; }

    public DataBaseType(string databaseType, string nullable)
    {
        DatabaseType = databaseType;
        Nullable = nullable;
    }
}
