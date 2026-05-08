using MiniOrm.Data.Metadata;
using Npgsql;
using System.Reflection;
using System.Text.Json;

namespace MiniOrm.Data;

/// <summary>
/// Represents a session with the database, providing access to entity sets and managing the underlying database
/// connection.
/// </summary>
/// <remarks>DbContext is responsible for configuring and managing the database connection, as well as
/// initializing entity sets represented by DbSet<T> properties. It provides methods to open and close the connection
/// asynchronously and supports both synchronous and asynchronous disposal patterns. This class is intended to be used
/// as a base for application-specific context classes that define entity sets for database operations.</remarks>
public class DbContext
{
    private readonly string _dbConnectionString;
    private NpgsqlConnection? _connection;
    private readonly ModelMetadata _model;

    public DbContext(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        InitializeDbsets();
        _model = MetadataProvider.GetModel(GetType());

        var obj = JsonSerializer.Serialize(_model);
        Console.WriteLine(obj);
    }

    /// <summary>
    /// Gets an open connection to the PostgreSQL database for executing commands.
    /// </summary>
    /// <remarks>The returned connection is not automatically opened. Callers are responsible for managing the
    /// connection's state and disposing of it when no longer needed. This method returns the same connection instance
    /// for each call, which may not be thread safe.</remarks>
    /// <returns>An instance of <see cref="NpgsqlConnection"/> representing the database connection. The same instance is
    /// returned on subsequent calls.</returns>
    public NpgsqlConnection GetConnection()
    {
        if (_connection == null)
        {
            _connection = new NpgsqlConnection(_dbConnectionString);
        }
        return _connection;
    }

    /// <summary>
    /// Asynchronously opens the database connection if it is not already open.
    /// </summary>
    /// <remarks>If the connection is already open, this method completes immediately without performing any
    /// action.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenConnectionAsync()
    {
        var connection = GetConnection();
        if(connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }

    /// <summary>
    /// Asynchronously closes the database connection if it is currently open.
    /// </summary>
    /// <remarks>If the connection is already closed, this method completes without performing any action.
    /// This method is thread-safe and can be awaited to ensure the connection is closed before proceeding.</remarks>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    public async Task CloseConnectionAsync()
    {
        var connection = GetConnection();
        if (connection.State == System.Data.ConnectionState.Open)
        {
            await connection.CloseAsync();
        }
    }

    /// <summary>
    /// Releases all resources used by the current instance of the class.
    /// </summary>
    /// <remarks>Call this method when you are finished using the object to free unmanaged resources
    /// immediately. After calling Dispose, the object should not be used.</remarks>
    public void Dispose()
    {
        _connection?.Dispose();
    }

    /// <summary>
    /// Asynchronously releases the resources used by the current instance.
    /// </summary>
    /// <remarks>Call this method to clean up resources when the instance is no longer needed. This method
    /// should be awaited to ensure that all resources are released before continuing execution.</remarks>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async Task DisposeAsync()
    {
        if( _connection != null )
            await _connection.DisposeAsync();
    }

    /// <summary>
    /// Initializes all public instance properties of type DbSet<T> on the current context.
    /// </summary>
    /// <remarks>This method uses reflection to identify and instantiate DbSet<T> properties. It should be
    /// called during context initialization to ensure that all entity sets are properly configured before
    /// use.</remarks>
    private void InitializeDbsets()
    {
        var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.PropertyType.IsGenericType)
                continue;

            var genericType = property.PropertyType.GetGenericTypeDefinition();

            if (genericType != typeof(DbSet<>))
                continue;

            var entityType = property.PropertyType.GetGenericArguments()[0];

            var dbSetInstance = Activator.CreateInstance(typeof(DbSet<>).MakeGenericType(entityType), this);

            property.SetValue(this, dbSetInstance);
        }
    }
}
