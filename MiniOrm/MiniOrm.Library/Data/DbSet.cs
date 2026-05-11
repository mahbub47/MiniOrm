using MiniOrm.MiniOrm.Library.Attributes;
using MiniOrm.Models;
using Npgsql;
using System.Reflection;

namespace MiniOrm.MiniOrm.Library.Data;

/// <summary>
/// This class represents a set of entities of a specific type, providing methods to query and manipulate the data.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="context"></param>
public class DbSet<TEntity>(DbContext context) where TEntity : new()
{
    public IEnumerable<Product> ToList()
    {
        return new List<Product>()
        {
            new Product{ Id = 1, Name = "Test", Price = 10m, Discount = 10m, InStock = true}
        };
    }

    /// <summary>
    /// This method inserts a new entity into the database and returns the generated primary key value.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<int> InsertAsync(TEntity entity)
    {
        using var conn = context.GetConnection();

        await conn.OpenAsync();

        var metadata = context.GetEntityMetadata(typeof(TEntity));

        var columns = metadata.Properties!.Where(p => p.IsPrimaryKey != true).ToList();

        var colList = string.Join(", ", columns.Select(p => p.Name));
        var paramList = string.Join(", ", columns.Select((p) => $"@{p.Name}"));
        var pkCol = metadata.Properties!.FirstOrDefault(p => p.IsPrimaryKey == true);

        var sql = $"INSERT INTO {metadata.Name} ({colList}) VALUES ({paramList}) RETURNING {pkCol!.Name};";

        using var cmd = new NpgsqlCommand(sql, conn);

        foreach ( var col in columns)
        {
            var propInfo = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()!.Name == col.Name || p.Name == col.Name);

            if(propInfo != null)
            {
                var value = propInfo.GetValue(entity);

                cmd.Parameters.AddWithValue(col.Name!, value ?? DBNull.Value);
            }
        }

        var result = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("Id is not returned");

        return Convert.ToInt32(result);
    }

    /// <summary>
    /// This method retrieves an entity from the database based on its primary key value. If no entity is found, it returns null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TEntity> FindByIdAsync(int id)
    {
        using var conn = context.GetConnection();

        await conn.OpenAsync();

        var metadata = context.GetEntityMetadata(typeof(TEntity));
        var pkCol = metadata.Properties!.FirstOrDefault(p => p.IsPrimaryKey == true);

        string sql = $"SELECT * FROM {metadata.Name} WHERE {pkCol!.Name} = @id LIMIT 1";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        var reader = await cmd.ExecuteReaderAsync();
        if (!reader.Read()) return default!;

        var entity = new TEntity();

        foreach(var col in metadata.Properties!)
        {
            var propInfo = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()!.Name == col.Name || p.Name == col.Name);

            var ordinal = reader.GetOrdinal(col.Name!);

            if (reader.IsDBNull(ordinal))
            {
                propInfo!.SetValue(entity, null);
            }
            else
            {
                var value = reader.GetValue(ordinal);
                propInfo!.SetValue(entity, Convert.ChangeType(value, col.ClrType!));
            }
        }

        return entity;
    }

    /// <summary>
    /// This method retrieves all entities of the specified type from the database and returns them as an enumerable collection.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        using var conn = context.GetConnection();

        await conn.OpenAsync();

        var metadata = context.GetEntityMetadata(typeof(TEntity));

        string sql = $"SELECT * FROM {metadata.Name}";

        using var cmd = new NpgsqlCommand(sql, conn);

        var reader = await cmd.ExecuteReaderAsync();

        var entities = new List<TEntity>();

        while (reader.Read())
        {
            var entity = new TEntity();

            foreach (var col in metadata.Properties!)
            {
                var propInfo = typeof(TEntity).GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()!.Name == col.Name || p.Name == col.Name);

                var ordinal = reader.GetOrdinal(col.Name!);

                if (reader.IsDBNull(ordinal))
                {
                    propInfo!.SetValue(entity, null);
                }
                else
                {
                    var value = reader.GetValue(ordinal);
                    propInfo!.SetValue(entity, Convert.ChangeType(value, col.ClrType!));
                }
            }

            entities.Add(entity);
        }

        return entities;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        using var conn = context.GetConnection();

        await conn.OpenAsync();

        var metadata = context.GetEntityMetadata(typeof(TEntity));

        var columns = metadata.Properties!.Where(p => p.IsPrimaryKey != true).ToList();

        var setClause = string.Join(",", columns.Select(p => $"{p.Name} = @{p.Name}"));
        var pkCol = metadata.Properties!.FirstOrDefault(p => p.IsPrimaryKey == true);

        string sql = $"UPDATE {metadata.Name} SET {setClause} WHERE {pkCol!.Name} = @{pkCol!.Name}";

        using var cmd = new NpgsqlCommand(sql, conn);

        foreach (var col in columns)
        {
            var propInfo = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()!.Name == col.Name || p.Name == col.Name);

            if (propInfo != null)
            {
                var value = propInfo.GetValue(entity);

                cmd.Parameters.AddWithValue(col.Name!, value ?? DBNull.Value);
            }
        }

        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// This method deletes an entity from the database based on its primary key value. If no entity is found with the specified id, no action is taken.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        using var conn = context.GetConnection();

        await conn.OpenAsync();

        var metadata = context.GetEntityMetadata(typeof(TEntity));

        var pkCol = metadata.Properties!.FirstOrDefault(p => p.IsPrimaryKey == true);

        string sql = $"DELETE FROM {metadata.Name} WHERE {pkCol!.Name} = @{pkCol!.Name}";

        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue($"@{pkCol.Name}", id);

        await cmd.ExecuteNonQueryAsync();
    }
}