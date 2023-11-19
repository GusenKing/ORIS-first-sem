using System.Data;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;


namespace MyOrm;

public class Orm : IOrmOperations
{
    private SqlConnectionStringBuilder _connectionStringBuilder;

    public Orm(string connectionString)
    {
        _connectionStringBuilder = new SqlConnectionStringBuilder(
            connectionString);
    }

    public Orm(SqlConnectionStringBuilder connectionStringBuilder)
    {
        _connectionStringBuilder = connectionStringBuilder;
    }
    
    /// <returns>True если произошло успешное добавление строки,
    /// False, если ни одна строка не изменилась </returns>
    public async Task<bool> Add<T>(T entity)
    {
        var entityType = entity?.GetType();
        var propertiesExceptId = entityType?
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !p.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var commandStringBuilder = new StringBuilder();
        commandStringBuilder.Append($"INSERT INTO {entityType?.Name} VALUES (");
        foreach (var property in propertiesExceptId)
            commandStringBuilder.Append($"\'{property.GetValue(entity)}\',");

        commandStringBuilder.Replace(",", ");", commandStringBuilder.Length - 1, 1);

        return await ConnectAndExecuteNonQueryAsync(commandStringBuilder.ToString());
    }

    /// <returns>True если произошло успешное изменение строки,
    /// False, если ни одна строка не изменилась </returns>
    public async Task<bool> Update<T>(T entity)
    {
        var entityType = entity?.GetType();
        var tableId = entityType?.GetProperty("Id")?.GetValue(entity);
        var propertiesExceptId = entityType?
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !p.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        var commandStringBuilder = new StringBuilder();
        commandStringBuilder.Append($"UPDATE {entityType?.Name} SET ");
        foreach (var property in propertiesExceptId)
            commandStringBuilder.Append($"{property.Name} = \'{property.GetValue(entity)}\',");
        
        commandStringBuilder.Replace(",", $" WHERE Id = {tableId};", commandStringBuilder.Length - 1, 1);
        Console.WriteLine(commandStringBuilder.ToString());
        return await ConnectAndExecuteNonQueryAsync(commandStringBuilder.ToString());
    }
    
    /// <returns>True если произошло успешное удаление строки,
    /// False, если ни одна строка не изменилась</returns>
    public async Task<bool> Delete<T>(int id)
    {
        var tableName = typeof(T).Name;
        var sqlExpression = $"DELETE FROM {tableName} WHERE Id = {id};";

        return await ConnectAndExecuteNonQueryAsync(sqlExpression);
    }

    public async Task<List<T>> Select<T>(T entity) where T : class, new()
    {
        var type = entity?.GetType();
        var tableName = type?.Name;
        List<T> entitiesList = new List<T>();

        var sqlQuery = $"SELECT * FROM \"{tableName.ToLower()}\"";
        await using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(sqlQuery, connection);
            var dataTable = new DataTable();
            dataTable.Load(await command.ExecuteReaderAsync());

            foreach (var row in dataTable.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    try
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType), null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Caught exception while querying database {ex.Message}");
                    }
                }
                entitiesList.Add(obj);
            }
            return entitiesList;
        }
    }

    public async Task<T> SelectById<T>(int id) where T : class, new()
    {
        var type = typeof(T);
        T obj = new T();

        var sqlQuery = $"SELECT * FROM {type.Name} WHERE Id = {id};";
        await using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand(sqlQuery, connection);
            var dataTable = new DataTable();
            dataTable.Load(await command.ExecuteReaderAsync());
            var dataRow = dataTable.AsEnumerable().First();
            
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    property.SetValue(obj, Convert.ChangeType(dataRow[property.Name], property.PropertyType));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Caught exception while querying database {ex.Message}");
                }
            }

            return obj;
        }
    }

    private async Task<bool> ConnectAndExecuteNonQueryAsync(string sqlExpression)
    {
        await using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand(sqlExpression, connection);
            var number = await command.ExecuteNonQueryAsync();
            return number != 0;
        }
    }
}