using MiniOrm.MiniOrm.Library.Data.Metadata;
using Npgsql;
using System.Reflection;

namespace MiniOrm.MiniOrm.Library.Data;

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
    private readonly ModelMetadata _model;

    public DbContext(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        InitializeDbsets();
        _model = MetadataProvider.GetModel(GetType());
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
        return new NpgsqlConnection(_dbConnectionString);
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

    /// <summary>
    /// This method retrieves the metadata for a given entity type from the model. It searches through the list of
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public EntityMetadata GetEntityMetadata(Type entityType)
    {
        return _model.Entities!.FirstOrDefault(e => e.EntityType == entityType)!;
    }
}
