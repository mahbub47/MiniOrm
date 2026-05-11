namespace MiniOrm.Migrations.Diff_Engine.Operations;

/// <summary>
/// This operation represents altering an existing column in a table. 
/// It contains the necessary information to identify the table and the column being altered, 
/// as well as the new definition of the column and its old definition for reference.
/// </summary>
public class AlterColumn : MigrationOperation
{
    // The name of the table containing the column to be altered.
    public string? TableName { get; set; }

    // The name of the column being altered.
    public string? ColumnName { get; set; }

    // The new definition of the column, including its new data type and any new constraints.
    public string? NewDataType { get; set; }

    // The old definition of the column, including its old data type and any old constraints, for reference when generating the down migration.
    public string? OldDataType { get; set; }
}
