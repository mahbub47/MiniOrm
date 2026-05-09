namespace MiniOrm.Data;

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
            return new DataBaseType("INT", false);

        if (clrType == typeof(int?))
            return new DataBaseType("INT", true);

        if (clrType == typeof(long))
            return new DataBaseType("BIGINT", false);

        if (clrType == typeof(long?))
            return new DataBaseType("BIGINT", true);

        if (clrType == typeof(float))
            return new DataBaseType("REAL", false);

        if (clrType == typeof(float?))
            return new DataBaseType("REAL", true);

        if (clrType == typeof(double?))
            return new DataBaseType("DOUBLE PRECISION", true);

        if (clrType == typeof(double))
            return new DataBaseType("DOUBLE PRECISION", false);

        if (clrType == typeof(decimal?))
            return new DataBaseType("NUMERIC", true);

        if (clrType == typeof(decimal))
            return new DataBaseType("NUMERIC", false);

        if (clrType == typeof(bool))
            return new DataBaseType("BOOLEAN", false);

        if (clrType == typeof(bool?))
            return new DataBaseType("BOOLEAN", true);

        if (clrType == typeof(DateTime?))
            return new DataBaseType("TIMESTAMP", true);

        if (clrType == typeof(DateTime))
            return new DataBaseType("TIMESTAMP", false);

        if (clrType == typeof(Guid?))
            return new DataBaseType("UUID", true);

        if (clrType == typeof(Guid))
            return new DataBaseType("UUID", false);

        if (!clrType.IsValueType)
        {
            if (clrType == typeof(string))
                return new DataBaseType("TEXT", false);
            else
                return new DataBaseType("TEXT", true);
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
    public bool IsNullable { get; set; }

    public DataBaseType(string databaseType, bool isNullable)
    {
        DatabaseType = databaseType;
        IsNullable = isNullable;
    }
}
