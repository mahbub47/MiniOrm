using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

public static class MetadataProvider
{
    private static readonly Dictionary<Type, ModelMetadata> _cache = new();

    private static readonly MetadataBuilder _builder = new();

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
