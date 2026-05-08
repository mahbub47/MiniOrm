using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

public class ModelMetadata
{
    public List<EntityMetadata>? Entities { get; set; } = new();
}
