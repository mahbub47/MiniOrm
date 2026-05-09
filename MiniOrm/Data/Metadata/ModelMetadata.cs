using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

/// <summary>
/// This class represents the metadata information for a model, which includes a collection of entities.
/// </summary>
public class ModelMetadata
{
    /// <summary>
    /// List of entities that are part of the model. Each entity contains metadata about its properties and structure.
    /// </summary>
    public List<EntityMetadata>? Entities { get; set; } = new();
}
