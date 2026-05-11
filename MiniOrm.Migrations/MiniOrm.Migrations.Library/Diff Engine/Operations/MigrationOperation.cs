namespace MiniOrm.Migrations.Diff_Engine.Operations;

/// <summary>
/// This is the base class for all migration operations. 
/// It serves as a common ancestor for specific operations such as creating tables,
/// dropping tables, adding columns, and dropping columns. 
/// Each specific operation will inherit from this base class and contain the necessary 
/// information to perform that operation on the database schema. This design allows for a 
/// structured and organized way to represent different types of schema changes in the migration process.
/// </summary>
public abstract class MigrationOperation
{
}
