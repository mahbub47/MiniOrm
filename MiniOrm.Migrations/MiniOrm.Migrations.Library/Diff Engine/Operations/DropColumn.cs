
namespace MiniOrm.Migrations.Diff_Engine.Operations;


/// <summary>
/// This operation represents dropping an existing column from a table.
/// </summary>
public class DropColumn : MigrationOperation
{
    // The name of the table from which the column is being dropped.
    public string? TableName { get; set; }

    // The name of the column being dropped.
    public string? ColumnName { get; set; }
}
