using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    public string? Name { get; set; }
    public string? DataBaseType { get; set; }

    public ColumnAttribute(string? Name = null, string? DataBaseType = null)
    {
        this.Name = Name;
        this.DataBaseType = DataBaseType;
    }
}
