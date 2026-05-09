using MiniOrm.Migrations.Diff_Engine.Operations;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace MiniOrm.Migrations.Sql_Generator;

public class SqlGenerator
{
    public StringBuilder GenerateSql(List<MigrationOperation> operations, StringBuilder sb)
    {
        foreach (var operation in operations)
        {
            if (operation is CreateTable createTable)
            {
                CreateTable(createTable, sb);
            }
            else if (operation is DropTable dropTable)
            {
                DropTable(dropTable, sb);
            }
            else if (operation is AddColumn addColumn)
            {
                AddColumn(addColumn, sb);
            }
            else if (operation is AlterColumn alterColumn)
            {
                AlterColumn(alterColumn, sb);
            }
            else if (operation is DropColumn dropColumn)
            {
                DropColumn(dropColumn, sb);
            }
        }
        return sb;
    }
    private StringBuilder CreateTable(CreateTable createTable, StringBuilder sb)
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

    private StringBuilder DropTable(DropTable dropTable, StringBuilder sb)
    {
        sb.AppendLine($"DROP TABLE IF EXISTS {dropTable.Table!.Name};");
        sb.AppendLine();
        return sb;
    }

    private StringBuilder AddColumn(AddColumn addColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {addColumn.TableName} ADD {addColumn.ColumnName} {addColumn.ColumnDefinition};");
        sb.AppendLine();
        return sb;
    }

    private StringBuilder AlterColumn(AlterColumn alterColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {alterColumn.TableName} ALTER COLUMN {alterColumn.ColumnName} TYPE {alterColumn.NewDataType};");
        sb.AppendLine();
        return sb;
    }

    private StringBuilder DropColumn(DropColumn dropColumn, StringBuilder sb)
    {
        sb.AppendLine($"ALTER TABLE {dropColumn.TableName} DROP COLUMN {dropColumn.ColumnName};");
        sb.AppendLine();
        return sb;
    }
}
