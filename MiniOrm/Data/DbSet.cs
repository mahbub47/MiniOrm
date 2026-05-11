using MiniOrm.Attributes;
using MiniOrm.Models;
using Npgsql;
using System.Reflection;
using System.Text;

namespace MiniOrm.Data;

/// <summary>
/// This class represents a set of entities of a specific type, providing methods to query and manipulate the data.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="context"></param>
public class DbSet<TEntity>(DbContext context) where TEntity : class
{
    public IEnumerable<Product> ToList()
    {
        return new List<Product>()
        {
            new Product{ Id = 1, Name = "Test", Price = 10m, Discount = 10m, InStock = true}
        };
    }

    public async Task<int> Insert(TEntity entity)
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
}