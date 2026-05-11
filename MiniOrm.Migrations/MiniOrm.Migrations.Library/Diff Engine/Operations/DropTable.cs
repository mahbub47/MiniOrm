using MiniOrm.MiniOrm.Library.Data.Metadata;

namespace MiniOrm.Migrations.Diff_Engine.Operations;

/// <summary>
/// This class represents the operation of dropping an existing table from the database.
/// </summary>
public class DropTable : MigrationOperation
{
    // The metadata of the table being dropped, including its name and properties.
    public EntityMetadata? Table { get; set; }
}
