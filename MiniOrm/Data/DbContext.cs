using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data;

public class DbContext(string dbConnectionString)
{
    private readonly string _dbConnectionString = dbConnectionString;
    private NpgsqlConnection? _connection;

    public NpgsqlConnection GetConnection()
    {
        if (_connection == null)
        {
            _connection = new NpgsqlConnection(_dbConnectionString);
        }
        return _connection;
    }

    public async Task OpenConnectionAsync()
    {
        var connection = GetConnection();
        if(connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }

    public async Task CloseConnectionAsync()
    {
        var connection = GetConnection();
        if (connection.State == System.Data.ConnectionState.Open)
        {
            await connection.CloseAsync();
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public async Task DisposeAsync()
    {
        if( _connection != null )
            await _connection.DisposeAsync();
    }

    public DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return new DbSet<TEntity>(this);
    }
}
