using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Plane;

public class ORM
{
    private string connectionString = "Server=localhost;Database=Planes;Trusted_Connection=True;Encrypt=False;";
    
    public async Task<List<T>> Select<T>(T entity) where T: class, new()
    {
        var type = entity?.GetType();
        var tableName = type?.Name;
        List<T> entitiesList = new List<T>();

        var sqlExpression = $"SELECT * FROM \"{tableName.ToLower()}\"";
        using (SqlConnection _connection = new SqlConnection(connectionString))
        {
            await _connection.OpenAsync();

            SqlCommand command = new SqlCommand(sqlExpression, _connection);
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
    }
}