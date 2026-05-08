using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableAttribute : Attribute
{
    public string? TableName { get; set; }
    public TableAttribute(string? Name = null)
    {
        TableName = Name;
    }
}
