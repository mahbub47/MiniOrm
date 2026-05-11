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
        if (clrType == null)
            return null!;

        if (clrType == typeof(int))
            return new DataBaseType("INT", "NOT NULL");

        if (clrType == typeof(int?))
            return new DataBaseType("INT", "NULL");

        if (clrType == typeof(long))
            return new DataBaseType("BIGINT", "NOT NULL");

        if (clrType == typeof(long?))
            return new DataBaseType("BIGINT", "NULL");

        if (clrType == typeof(float))
            return new DataBaseType("REAL", "NOT NULL");

        if (clrType == typeof(float?))
            return new DataBaseType("REAL", "NULL");

        if (clrType == typeof(double?))
            return new DataBaseType("DOUBLE PRECISION", "NULL");

        if (clrType == typeof(double))
            return new DataBaseType("DOUBLE PRECISION", "NOT NULL");

        if (clrType == typeof(decimal?))
            return new DataBaseType("NUMERIC", "NULL");

        if (clrType == typeof(decimal))
            return new DataBaseType("NUMERIC", "NOT NULL");

        if (clrType == typeof(bool))
            return new DataBaseType("BOOLEAN", "NOT NULL");

        if (clrType == typeof(bool?))
            return new DataBaseType("BOOLEAN", "NULL");

        if (clrType == typeof(DateTime?))
            return new DataBaseType("TIMESTAMP", "NULL");

        if (clrType == typeof(DateTime))
            return new DataBaseType("TIMESTAMP", "NOT NULL");

        if (clrType == typeof(Guid?))
            return new DataBaseType("UUID", "NULL");

        if (clrType == typeof(Guid))
            return new DataBaseType("UUID", "NOT NULL");

        if (!clrType.IsValueType)
        {
            if (clrType == typeof(string))
                return new DataBaseType("TEXT", "NOT NULL");
            else
                return new DataBaseType("TEXT", "NULL");
        }

        return null!;
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
