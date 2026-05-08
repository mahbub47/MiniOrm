using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MiniOrm.Data.Metadata;

public class PropertyMetadata
{
    public string? Name { get; set; }

    [JsonIgnore]
    public Type? ClrType { get; set; }
    public string? DatabaseType { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; }
}
