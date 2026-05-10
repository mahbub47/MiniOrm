using MiniOrm.Migrations.Database;
using MiniOrm.Migrations.Diff_Engine;
using System.Text;

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
    private readonly SqlManager _sqlManager;
    private readonly DiffEngine _diffEngine;
    private readonly string _connectionString;
    private readonly string _migrationDir;
    public MigrationRunner()
    {
        _connectionString = "Host=localhost;Username=postgres;Password=MyPGServer;Database=miniOrm";
        _diffEngine = new DiffEngine();
        _sqlManager = new SqlManager(_connectionString);

        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        _migrationDir = Path.Combine(projectRoot, "Migrations");
        Directory.CreateDirectory(_migrationDir);
    }

    /// <summary>
    /// Runs the migration command based on the provided arguments. It supports commands for creating new migrations,
    /// </summary>
    /// <param name="args"></param>
    public async Task Run(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("command not found run `dotnet run -- help` ");
            return;
        }
        else if (args.Length == 1 && args[0] == "help")
        {
            Help();
            return;
        }
        else if (args.Length == 2)
        {
            if (args[0] == "migrations" && args[1] == "apply")
            {
                await ApplyMigrations();
            }
            else if(args[0] == "migrations" && args[1] == "list")
            {
                await MigrationList();
            }
            else if (args[0] == "migrations" && args[1] == "rollback")
            {
                await MigrationRollback();
            }
            else
            {
                Console.WriteLine("command not found run `dotnet run -- help` ");
                return;
            }
        }
        else if (args.Length == 3)
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
        else
        {
            Console.WriteLine("command not found run `dotnet run -- help` ");
            return;
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
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string fileName = $"{timestamp}_{name}.sql";
        string filePath = Path.Combine(_migrationDir, fileName);

        var sb = new StringBuilder();

        var upOperations = _diffEngine.GenerateUpOperations();

        sb.AppendLine("-- up");

        _sqlManager.GenerateSql(upOperations, sb);

        sb.AppendLine();
        sb.AppendLine("-- down");

        var downOperations = _diffEngine.GetDownOperations(upOperations);

        _sqlManager.GenerateSql(downOperations, sb);

        File.WriteAllText(filePath, sb.ToString());

        _diffEngine.UpdateSnapshot();

        Console.WriteLine($"Migration created: {fileName}");
        Console.WriteLine($"Location: {filePath}");
    }

    /// <summary>
    /// This method applies all pending migrations by executing the "up" SQL scripts of the migration files that have not been applied yet.
    /// </summary>
    /// <returns></returns>
    private async Task ApplyMigrations()
    {
        await _sqlManager.CreateMigrationTable();

        var appliedMigrations = await _sqlManager.GetAppliedMigrations();

        var files = GetLocalMigrations();

        int count = 0;

        foreach(var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            if (appliedMigrations.Contains(name))
                continue;

            Console.WriteLine($"Applying {name}...");
            var content = File.ReadAllText(file);

            await _sqlManager.ExecuteUpAsync(content);

            await _sqlManager.UploadMigrationAsync(name);

            Console.WriteLine($"Applied {name}.");
            count++;
        }

        Console.WriteLine(count == 0 ? "No migrations applied" : $"{count} migration(s) applied");
    }

    /// <summary>
    /// This method lists all migration files in the "Migrations" directory and indicates whether 
    /// each migration has been applied or is still pending based on the records in the database.
    /// </summary>
    /// <returns></returns>
    private async Task MigrationList()
    {
        var appliedMigrations = await _sqlManager.GetAppliedMigrations();
        var localMigrationFiles = GetLocalMigrations();
        foreach(var file in localMigrationFiles)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            var isApplied = appliedMigrations.Contains(name);
            Console.Write(isApplied ? "[Applied]" : "[Pending]");
            Console.Write($" {name}");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// This method rolls back the last applied migration by executing the "down" SQL script of the most recently applied migration file.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task MigrationRollback()
    {
        var lastAppliedMigration = await _sqlManager.GetLastAppliedMigration();

        if (string.IsNullOrEmpty(lastAppliedMigration))
            throw new InvalidOperationException("No applied migration to rollback");

        var localMigrationFiles = GetLocalMigrations();

        foreach (var file in localMigrationFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            if (name != lastAppliedMigration) continue;

            Console.WriteLine($"Rollback {name}...");
            var content = File.ReadAllText(file);

            await _sqlManager.ExecuteDownAsync(content);

            await _sqlManager.RollbackMigrationAsync(name);

            Console.WriteLine($"Rollback to {name}.");
        }
    }

    /// <summary>
    /// This method retrieves all migration files from the "Migrations" directory, orders them by their names (which include timestamps), and returns the list of file paths.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private IEnumerable<string> GetLocalMigrations()
    {
        var localFiles = Directory.GetFiles(_migrationDir, "*.sql").OrderBy(f => f);

        if (!localFiles.Any())
            throw new FileNotFoundException("No migration found to be applied");

        return localFiles;
    }
}
