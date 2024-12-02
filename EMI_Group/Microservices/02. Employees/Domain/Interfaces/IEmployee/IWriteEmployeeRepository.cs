using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IEmployee
{
    public interface IWriteEmployeeRepository
    {
        Task<(bool success, string result)> Create(Employee employee, DateTime startDate, DateTime? endDate);
        Task<(bool success, string result)> Update(Employee employee, DateTime startDate, DateTime? endDate);
    }
}
