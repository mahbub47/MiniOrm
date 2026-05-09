using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

/// <summary>
/// This class serves as a provider for metadata information about a given context type. 
/// It maintains a cache of metadata to optimize performance and avoid redundant computations. 
/// When requested for metadata of a specific context type, it first checks the cache; 
/// if the metadata is not present, it uses the MetadataBuilder to construct the metadata, 
/// stores it in the cache, and then returns it. This approach ensures efficient retrieval of metadata 
/// while minimizing overhead associated with building metadata multiple times for the same context type.
/// </summary>
public static class MetadataProvider
{
    private static readonly Dictionary<Type, ModelMetadata> _cache = new();

    private static readonly MetadataBuilder _builder = new();

    /// <summary>
    /// This method retrieves the metadata for a given context type. It first checks if the metadata is already cached;
    /// </summary>
    /// <param name="contextType"></param>
    /// <returns></returns>
    public static ModelMetadata GetModel(Type contextType)
    {
        if (_cache.TryGetValue(contextType, out var model))
        {
            return model;
        }

        model = _builder.Build(contextType);
        _cache[contextType] = model;
        return model;
    }
}
