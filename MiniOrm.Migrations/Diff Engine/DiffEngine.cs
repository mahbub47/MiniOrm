using MiniOrm.Data.Metadata;
using MiniOrm.Migrations.Diff_Engine.Operations;
using System.Text.Json;

namespace MiniOrm.Migrations.Diff_Engine;

/// <summary>
/// This class is responsible for comparing the current model metadata with a snapshot of the previous model metadata 
/// to generate a list of migration operations needed to update the database schema.
/// </summary>
public class DiffEngine
{
    // Retrieves the snapshot model metadata from a JSON file. The snapshot represents the state of the model at the time of the last migration.
    private ModelMetadata GetSnapshotModel()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string snapshotDir = Path.Combine(projectRoot, "Snapshot/model_snapshot.json");
        string json = File.ReadAllText(snapshotDir);
        ModelMetadata snapshotModel = JsonSerializer.Deserialize<ModelMetadata>(json)!;
        return snapshotModel;
    }

    // Compares the current model metadata with the snapshot model metadata to generate a list of migration operations needed to update the database schema.
    public List<MigrationOperation> GenerateUpOperations()
    {
        var upOperations = new List<MigrationOperation>();
        var currentModel = MetadataProvider.GetModel(typeof(AppDbContext));
        var snapshotModel = GetSnapshotModel();
        foreach(var entity in currentModel.Entities!)
        {
            var existTable = snapshotModel.Entities?.FirstOrDefault(e => e.Name == entity.Name);
            if(existTable == null)
            {
                upOperations.Add(new CreateTable
                {
                    Table = entity
                });
            }else
            {
                CompareColumn(entity, existTable, upOperations);
            }
        }

        foreach(var entity in snapshotModel.Entities!)
        {
            if(!currentModel.Entities.Any(e => e.Name == entity.Name))
            {
                upOperations.Add(new DropTable
                {
                    Table = entity
                });
            }
        }
        return upOperations;
    }

    // Generates a list of migration operations needed to revert the changes made by the "up" operations.
    // This is used for rolling back migrations if needed.
    public List<MigrationOperation> GetDownOperations(List<MigrationOperation> ups)
    {
        var downOperations = new List<MigrationOperation>();

        foreach (var upOperation in ups)
        {
            if (upOperation is CreateTable createTable)
            {
                downOperations.Add(new DropTable
                {
                    Table = createTable.Table
                });
            }
            else if (upOperation is AddColumn addColumn)
            {
                downOperations.Add(new DropColumn
                {
                    TableName = addColumn.TableName,
                    ColumnName = addColumn.ColumnName
                });
            }
            else if (upOperation is AlterColumn alterColumn)
            {
                downOperations.Add(new AlterColumn
                {
                    TableName = alterColumn.TableName,
                    ColumnName = alterColumn.ColumnName,
                    NewDataType = alterColumn.OldDataType,
                    OldDataType = alterColumn.NewDataType
                });
            }
        }

        return downOperations;
    }

    // Compares the columns of the current entity with the columns of the old entity to identify any differences in column definitions,
    // such as added columns, removed columns, or altered column types. It generates the appropriate migration operations based on these differences.
    private void CompareColumn(EntityMetadata current, EntityMetadata old, List<MigrationOperation> upOperation)
    {
        foreach (var currentCol in current.Properties!)
        {
            var oldCol = old.Properties?.FirstOrDefault(e => e.Name == currentCol.Name);

            if(oldCol == null)
            {
                upOperation.Add(new AddColumn
                {
                    TableName = current.Name,
                    ColumnName = currentCol.Name,
                    ColumnDefinition = currentCol.DatabaseType + " " + currentCol.Nullable
                });
            }else
            {
                if(oldCol.DatabaseType != currentCol.DatabaseType)
                {
                    upOperation.Add(new AlterColumn
                    {
                        TableName = current.Name,
                        ColumnName = currentCol.Name,
                        NewDataType = currentCol.DatabaseType,
                        OldDataType = oldCol.DatabaseType
                    });
                }
            }
        }
    }
}
