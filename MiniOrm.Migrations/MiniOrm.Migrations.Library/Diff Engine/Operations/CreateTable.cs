using MiniOrm.MiniOrm.Library.Data.Metadata;

namespace MiniOrm.Migrations.Diff_Engine.Operations;

/// <summary>
/// This class represents the operation of creating a new table in the database. 
/// It contains a property "Table" which holds the metadata of the table to be created, 
/// including its name and properties. This operation is used when a new entity is added to the model 
/// that does not exist in the snapshot model, indicating that a new table needs to be created in the database to accommodate this entity.
/// </summary>
public class CreateTable : MigrationOperation
{
    // The metadata of the table to be created, including its name and properties.
    public EntityMetadata? Table { get; set; }
}
