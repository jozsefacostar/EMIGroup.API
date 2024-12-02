using Models;

namespace Repositories
{
    public interface IErrorCollection
    {
        Task<List<Error>> GetAllErrors();
        Task<Error> GetErrorById(string iderror);
        Task InsertError(Error error);
        Task UpdateError(Error error);
        Task DeleteError(string id);
    }
}
