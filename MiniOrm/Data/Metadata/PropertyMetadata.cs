using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

public class PropertyMetadata
{
    public string? Name { get; set; }
    public Type? ClrType { get; set; }
    public string? DatabaseType { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; }
}
