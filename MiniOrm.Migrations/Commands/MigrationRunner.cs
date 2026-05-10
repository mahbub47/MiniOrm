using MiniOrm.Migrations.Diff_Engine;
using MiniOrm.Migrations.Sql_Generator;

namespace MiniOrm.Migrations.Commands;

/// <summary>
/// MigrationRunner is responsible for handling migration commands such as creating new migrations, 
/// applying pending migrations, listing all migrations, and rolling back the last migration. 
/// It uses the DiffEngine to generate the necessary operations for the migrations 
/// and the SqlGenerator to create the SQL scripts based on those operations. 
/// The generated migration files are stored in a "Migrations" directory within the project, 
/// and each migration file contains both the "up" and "down" SQL scripts for applying and rolling back the migration, respectively.
/// </summary>
public class MigrationRunner
{
    private readonly SqlGenerator _sqlGenerator;
    private readonly DiffEngine _diffEngine;
    public MigrationRunner()
    {
        _sqlGenerator = new SqlGenerator();
        _diffEngine = new DiffEngine();
    }

    /// <summary>
    /// Runs the migration command based on the provided arguments. It supports commands for creating new migrations,
    /// </summary>
    /// <param name="args"></param>
    public void Run(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("command not found run `dotnet run -- help` ");
            return;
        }

        if (args.Length == 1 && args[0] == "help")
        {
            Help();
            return;
        }

        if (args.Length == 3)
        {
            if (args[0] == "migrations" && args[1] == "add")
            {
                var fileName = args[2];
                AddMigration(fileName);
                return;
            }
            else
            {
                Console.WriteLine("command not found run `dotnet run -- help` ");
                return;
            }
        }
    }

    // Displays the available migration commands and their usage instructions to the console.
    private void Help()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("dotnet run -- migrations add <FileName>      - Create migrations");
        Console.WriteLine("dotnet run -- migrations apply               - Apply all pending migraions");
        Console.WriteLine("dotnet run -- migrations lists               - shows list of all migrations");
        Console.WriteLine("dotnet run -- migrations rollback            - rollback last migrations");
    }

    /// <summary>
    /// This method generates a new migration file based on the differences between the current model and the last snapshot of the model.
    /// </summary>
    /// <param name="name">The name of the migration.</param>
    private void AddMigration(string name)
    {
        var upOperations = _diffEngine.GenerateUpOperations();

        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        string fileName = $"{timestamp}_{name}.sql";

        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        string migrationsDir = Path.Combine(projectRoot, "Migrations");

        Directory.CreateDirectory(migrationsDir);

        string filePath = Path.Combine(migrationsDir, fileName);

        var sb = new System.Text.StringBuilder();

        sb.AppendLine("-- up");

        _sqlGenerator.GenerateSql(upOperations, sb);

        sb.AppendLine();
        sb.AppendLine("-- down");

        var downOperations = _diffEngine.GetDownOperations(upOperations);

        _sqlGenerator.GenerateSql(downOperations, sb);

        File.WriteAllText(filePath, sb.ToString());

        _diffEngine.UpdateSnapshot();

        Console.WriteLine($"Migration created: {fileName}");
        Console.WriteLine($"Location: {filePath}");
    }
}
