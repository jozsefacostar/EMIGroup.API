using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IEmployeeTransactions
{
    public interface IEmployeeTransactionRepository
    {
        Task AnnualCalculatedIncreaseAllEmployee();
        Task<(bool success, string result)> RegisterEmployeeProject(EmployeeProject employeeProject);
    }
}
