using MiniOrm.Migrations.Diff_Engine.Operations;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace MiniOrm.Migrations.Sql_Generator;

/// <summary>
/// This class is responsible for generating SQL scripts based on a list of migration operations.
/// </summary>
public class SqlGenerator
{

    // The following private methods generate specific SQL statements for each type of migration operation.
    public StringBuilder CreateTable(CreateTable createTable, StringBuilder sb)
    {
        sb.AppendLine($"CREATE TABLE IF NOT EXISTS {createTable.Table!.Name} (");

        var colDefs = new List<string>();

        foreach (var prop in createTable.Table.Properties!)
        {
            if (prop.IsPrimaryKey)
            {
                colDefs.Add($"  {prop.Name} SERIAL PRIMARY KEY");
            }
            else
            {
                colDefs.Add($"  {prop.Name} {prop.DatabaseType} {prop.Nullable}");
            }
        }

        sb.AppendLine(string.Join(",\n", colDefs));
        sb.AppendLine(");");
        sb.AppendLine();
        return sb;
    }

    // The DropTable method generates a SQL statement to drop a table if it exists.
    public StringBuilder DropTable(DropTable dropTable, StringBuilder sb)
    {
        sb.AppendLine($"DROP TABLE IF EXISTS {dropTable.Table!.Name};");
        sb.AppendLine();
        return sb;
    }


    // The AddColumn method generates a SQL statement to add a new column to an existing table.
    public StringBuilder AddColumn(AddColumn addColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {addColumn.TableName} ADD {addColumn.ColumnName} {addColumn.ColumnDefinition};");
        sb.AppendLine();
        return sb;
    }

    // The AlterColumn method generates a SQL statement to alter the data type of an existing column in a table.
    public StringBuilder AlterColumn(AlterColumn alterColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {alterColumn.TableName} ALTER COLUMN {alterColumn.ColumnName} TYPE {alterColumn.NewDataType};");
        sb.AppendLine();
        return sb;
    }

    // The DropColumn method generates a SQL statement to drop a column from an existing table.
    public StringBuilder DropColumn(DropColumn dropColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {dropColumn.TableName} DROP COLUMN {dropColumn.ColumnName};");
        sb.AppendLine();
        return sb;
    }
}
