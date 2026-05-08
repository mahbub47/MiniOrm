using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    public string? ColumnName { get; set; }
    public string? DbDataType { get; set; }

    public ColumnAttribute(string? Name = null, string? DataType = null)
    {
        ColumnName = Name;
        DbDataType = DataType;
    }
}
