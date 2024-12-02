using Domain.Entities;
using Domain.Interfaces.IEmployeeTransactions;
using Microsoft.EntityFrameworkCore;
using System;
namespace Infraestructure.Persistence.Repositories
{
    public class EmployeeTransactionRepository : IEmployeeTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        #region ctor

        public EmployeeTransactionRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Private Methods
        /* Función que valida si un empleado existe */
        private async Task<bool> ExistEmployee(int id) => await _context.Employees.AsNoTracking().AnyAsync(x => x.Id.Equals(id));

        /* Función que valida si un empleado existe */
        private async Task<bool> ExistEmployeeAndProject(int employee, int project) => await _context.EmployeeProjects.AsNoTracking().AnyAsync(x => x.Employee.Equals(employee) && x.Project.Equals(project));

        /* Función que valida si una posición existe */
        private async Task<bool> ExistProject(int employee) => await _context.Projects.AsNoTracking().AnyAsync(x => x.Id.Equals(employee));
        #endregion

        #region Public Methods
        /* Función que registra el incremento anual para los empleados */
        public async Task AnnualCalculatedIncreaseAllEmployee() => await _context.Employees
        .Where(x => x.CurrentPosition != null)
        .Join(_context.Positions, employee => employee.CurrentPosition, position => position.Id, (employee, position) => new { Employee = employee, Position = position })
        .ExecuteUpdateAsync(u => u.SetProperty(
            e => e.Employee.Salary,
            e => e.Position.RegularJob ? e.Employee.Salary + e.Employee.Salary * 0.1m : e.Employee.Salary + e.Employee.Salary * 0.2m
        ));
        /* Función que registra un project a un empleado */
        public async Task<(bool success, string result)> RegisterEmployeeProject(EmployeeProject employeeProject)
        {
            if (!await ExistEmployee(employeeProject.Employee) || !await ExistProject(employeeProject.Project))
                return (false, "Por favor verifique los datos, no son correctos");

            if (await ExistEmployeeAndProject(employeeProject.Employee, employeeProject.Project))
                return (false, "El empleado y el proyecto ya están relacionados");

            await _context.EmployeeProjects.AddAsync(employeeProject);
            return (true, string.Empty);
        }
        #endregion




    }
}
