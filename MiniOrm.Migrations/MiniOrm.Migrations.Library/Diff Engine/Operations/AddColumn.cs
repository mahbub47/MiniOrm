namespace MiniOrm.Migrations.Diff_Engine.Operations;

/// <summary>
/// This operation represents adding a new column to an existing table. 
/// It contains the necessary information to identify the table and the column being added, 
/// as well as the definition of the new column.
/// </summary>
public class AddColumn : MigrationOperation
{
    // The name of the table to which the column is being added.
    public string? TableName { get; set; }

    // The name of the column being added.
    public string? ColumnName { get; set; }

    // The definition of the new column, including its data type and any constraints.
    public string? ColumnDefinition { get; set; }
}
