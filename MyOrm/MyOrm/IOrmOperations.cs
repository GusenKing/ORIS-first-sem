namespace MyOrm;

public interface IOrmOperations
{
    Task<bool> Add<T>(T entity);
    Task<bool> Update<T>(T entity);
    Task<bool> Delete<T>(int id);
    Task<List<T>> Select<T>(T entity) where T : class, new();
    Task<T> SelectById<T>(int id);
}
