using Domain.Entities;
namespace Domain.Interfaces.IEmployee
{
    public interface IReadEmployeeRepository
    {
        Task<List<Employee>> GetAll(int? pageNumber = null, int? pageSize = null);
        Task<Employee> GetById(string IdNum);
        Task<List<Employee>> GetEmployeesByDepartmentAndProjectQuery(string code);
    }
}
