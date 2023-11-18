using System.Data;
using System.Reflection;
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

    public Task<bool> Add<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete<T>(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> Select<T>(T entity) where T : class, new()
    {
        var type = entity?.GetType();
        var tableName = type?.Name;
        List<T> entitiesList = new List<T>();

        var sqlExpression = $"SELECT * FROM \"{tableName.ToLower()}\"";
        await using SqlConnection connection = new SqlConnection(_connectionStringBuilder.ConnectionString);
        await connection.OpenAsync();

        SqlCommand command = new SqlCommand(sqlExpression, connection);
        var dataTable = new DataTable();
        dataTable.Load(await command.ExecuteReaderAsync());

        foreach (var row in dataTable.AsEnumerable())
        {
            T obj = new T();

            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                }
                catch
                {
                    continue;
                }
            }

            entitiesList.Add(obj);
        }

        return entitiesList;
    }

    public Task<T> SelectById<T>(int id)
    {
        throw new NotImplementedException();
    }
}