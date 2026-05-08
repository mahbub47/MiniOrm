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
