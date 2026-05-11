namespace MiniOrm.MiniOrm.Library.Data;

/// <summary>
/// This class is responsible for mapping CLR types to corresponding database types. 
/// It provides a method to map a given CLR type to a database type, which includes the database type name and whether the type is nullable. 
/// This mapping is essential for generating SQL commands and defining database schemas based on the properties of entity classes in the application.
/// </summary>
public class TypeMapper
{
    public DataBaseType Map(Type clrType)
    {
        if (clrType == null) return null!;

        bool isNullable = !clrType.IsValueType || Nullable.GetUnderlyingType(clrType) != null;

        Type coreType = Nullable.GetUnderlyingType(clrType) ?? clrType;

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
            _ => throw new ArgumentException("Unknown Type to map")
        };

        string nullability = isNullable ? "NULL" : "NOT NULL";

        return new DataBaseType(sqlTypeName, nullability);
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
