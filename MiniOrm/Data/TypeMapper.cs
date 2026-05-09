using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data;

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
