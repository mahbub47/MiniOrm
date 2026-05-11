namespace MiniOrm.MiniOrm.Library.Data.Metadata;

/// <summary>
/// This class represents the metadata information for a model, which includes a collection of entities.
/// </summary>
public class ModelMetadata
{
    // It represents all the database tables in the model.
    public List<EntityMetadata>? Entities { get; set; } = new();
}
