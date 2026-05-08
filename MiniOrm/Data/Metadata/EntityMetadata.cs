using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

public class EntityMetadata
{
    public string? Name { get; set; }
    public List<PropertyMetadata>? Properties { get; set; } = new();
}
