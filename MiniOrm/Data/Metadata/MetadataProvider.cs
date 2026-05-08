using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data.Metadata;

public class MetadataProvider
{
    private readonly Dictionary<Type, ModelMetadata> _models;

    private readonly MetadataBuilder _builder;

    public MetadataProvider()
    {
        _builder = new MetadataBuilder();
    }
}
