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

        var type = Nullable.GetUnderlyingType(clrType);

        if (type == typeof(int))
            return new DataBaseType("INT", false);

        if (type == typeof(int?))
            return new DataBaseType("INT", true);

        if (type == typeof(long))
            return new DataBaseType("BIGINT", false);

        if (type == typeof(long?))
            return new DataBaseType("BIGINT", true);

        if (type == typeof(float))
            return new DataBaseType("REAL", false);

        if (type == typeof(float?))
            return new DataBaseType("REAL", true);

        if (type == typeof(double?))
            return new DataBaseType("DOUBLE PRECISION", true);

        if (type == typeof(double))
            return new DataBaseType("DOUBLE PRECISION", false);

        if (type == typeof(decimal?))
            return new DataBaseType("NUMERIC", true);

        if (type == typeof(decimal))
            return new DataBaseType("NUMERIC", false);

        if (type == typeof(bool))
            return new DataBaseType("BOOLEAN", false);

        if (type == typeof(bool?))
            return new DataBaseType("BOOLEAN", true);

        if (type == typeof(DateTime?))
            return new DataBaseType("TIMESTAMP", true);

        if (type == typeof(DateTime))
            return new DataBaseType("TIMESTAMP", false);

        if (type == typeof(Guid?))
            return new DataBaseType("UUID", true);

        if (type == typeof(Guid))
            return new DataBaseType("UUID", false);

        if (!clrType.IsValueType)
        {
            if (type == typeof(string))
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
