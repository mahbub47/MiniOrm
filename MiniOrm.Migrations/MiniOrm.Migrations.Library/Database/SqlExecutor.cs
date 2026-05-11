using Npgsql;

namespace MiniOrm.Migrations.MiniOrm.Migrations.Library.Database;

/// <summary>
/// This class is responsible for executing SQL commands against the database. It provides methods for executing non-query SQL commands,
/// </summary>
public class SqlExecutor
{
    private readonly string _connectionString;

    public SqlExecutor() => _connectionString = Environment.GetEnvironmentVariable("MINIORM_CONN")!;

    public async Task ExecuteNonQueryAsync(string sql)
    {
        if(string.IsNullOrEmpty(sql)) return;

        Console.WriteLine("SQL --- ");
        Console.WriteLine(sql);
        Console.WriteLine("---");

        using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<HashSet<string>> GetAllMigrationsAsync(string sql)
    {
        var set = new HashSet<string>();
        using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);

        var reader = await cmd.ExecuteReaderAsync();

        while (reader.Read())
        {
            set.Add(reader.GetString(0));
        }

        return set;
    }

    public async Task<string> GetLastMigrationAsync(string sql)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand(sql, conn);
        return await cmd.ExecuteScalarAsync() as string ?? "";
    }

    public async Task InsertMigrationAsync(string name)
    {
        using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();

        using var record = new NpgsqlCommand(
                "INSERT INTO __migrations (name, applied_at) VALUES (@n, @t)", conn);

        record.Parameters.AddWithValue("@n", name);
        record.Parameters.AddWithValue("@t", DateTime.UtcNow);

        await record.ExecuteNonQueryAsync();
    }

    public async Task DeleteMigrationAsync(string name)
    {
        using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();

        using var record = new NpgsqlCommand(
                "DELETE FROM __migrations WHERE name = @n;", conn);

        record.Parameters.AddWithValue("@n", name);

        await record.ExecuteNonQueryAsync();
    }
    
}
