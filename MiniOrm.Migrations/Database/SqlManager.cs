
using MiniOrm.Migrations.Diff_Engine.Operations;
using MiniOrm.Migrations.Sql_Generator;
using Npgsql;
using System.Net.WebSockets;
using System.Text;

namespace MiniOrm.Migrations.Database;

public class SqlManager
{
    private readonly SqlGenerator _generator;
    private readonly SqlExecutor _executor;

    public SqlManager(string connectionString)
    {
        _generator = new SqlGenerator();
        _executor = new SqlExecutor(connectionString);
    }

    // The GenerateSql method takes a list of migration operations and a StringBuilder to accumulate the generated SQL script.
    public StringBuilder GenerateSql(List<MigrationOperation> operations, StringBuilder sb)
    {
        foreach (var operation in operations)
        {
            if (operation is CreateTable createTable)
            {
                _generator.CreateTable(createTable, sb);
            }
            else if (operation is DropTable dropTable)
            {
                _generator.DropTable(dropTable, sb);
            }
            else if (operation is AddColumn addColumn)
            {
                _generator.AddColumn(addColumn, sb);
            }
            else if (operation is AlterColumn alterColumn)
            {
                _generator.AlterColumn(alterColumn, sb);
            }
            else if (operation is DropColumn dropColumn)
            {
                _generator.DropColumn(dropColumn, sb);
            }
        }
        return sb;
    }

    public async Task<HashSet<string>> GetAppliedMigrations()
    {
        string sql = "SELECT name FROM __migrations";

        var list = await _executor.GetAllMigrationsAsync(sql);

        return list;
    }

    public async Task<string> GetLastAppliedMigration()
    {
        string sql = "SELECT name FROM __migrations ORDER BY applied_at DESC LIMIT 1";
        return await _executor.GetLastMigrationAsync(sql);
    }

    public async Task CreateMigrationTable()
    {
        string sql = """
            CREATE TABLE IF NOT EXISTS __migrations (
                id         SERIAL PRIMARY KEY,
                name       TEXT NOT NULL UNIQUE,
                applied_at TIMESTAMP NOT NULL
            )
            """;
        await _executor.ExecuteNonQueryAsync(sql);
    }

    public async Task ExecuteUpAsync(string? content)
    {
        var sql = ExtractSection(content, "up");
        await _executor.ExecuteNonQueryAsync(sql);
    }

    public async Task ExecuteDownAsync(string? content)
    {
        var sql = ExtractSection(content, "down");
        await _executor.ExecuteNonQueryAsync(sql);
    }

    public async Task UploadMigrationAsync(string name)
    {
        await _executor.InsertMigrationAsync(name);
    }

    public async Task RollbackMigrationAsync(string name)
    {
        await _executor.DeleteMigrationAsync(name);
    }

    private static string ExtractSection(string? content, string section)
    {
        if (content == null) return string.Empty;

        string marker = $"-- {section}";
        int start = content.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (start < 0) return string.Empty;

        start += marker.Length;

        int next = content.IndexOf("\n-- ", start, StringComparison.OrdinalIgnoreCase);
        string block = next < 0 ? content[start..] : content[start..next];
        return block.Trim();
    }
}
