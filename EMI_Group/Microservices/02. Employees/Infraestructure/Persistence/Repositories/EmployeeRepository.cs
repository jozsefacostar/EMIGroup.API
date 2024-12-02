using Domain.Entities;
using Domain.Interfaces.IEmployee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infraestructure.Persistence.Repositories
{
    public class EmployeeRepository : IWriteEmployeeRepository, IReadEmployeeRepository, IDeleteEmployeeRepository
    {
        #region Variable
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctor
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Public Methods
        /* Función que consulta la información general de los empleados */
        public async Task<List<Employee>> GetAll(int? pageNumber = null, int? pageSize = null)
        {
            // Inicia la consulta con AsNoTracking para mejorar el rendimiento en lecturas
            var queryEmployees = _context.Employees.AsNoTracking();

            // Aplica la paginación si se especifican los parámetros
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                queryEmployees = queryEmployees.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            // Selecciona sólo los datos que necesitas (proyección)
            queryEmployees = queryEmployees.Select(e => new Employee
            {
                Id = e.Id,
                IdNum = e.IdNum,
                Name = e.Name,
                Salary = e.Salary,
                // Proyección de la propiedad relacionada con la posición
                CurrentPositionNavigation = new Position { Name = e.CurrentPositionNavigation.Name },
                LstPositionHistoryEmployee = e.LstPositionHistoryEmployee
            });

            // Ejecuta la consulta en la base de datos y materializa los resultados como List<Employee>
            return await queryEmployees.ToListAsync();
        }
        /* Función que consulta por Identificación la información relacionada a un empleado  */
        public async Task<Employee> GetById(string idNum)
        {
            var queryEmployees = _context.Employees.AsNoTracking().Where(x => x.IdNum.Equals(idNum));
            queryEmployees = queryEmployees.Select(e => new Employee
            {
                Id = e.Id,
                IdNum = e.IdNum,
                Name = e.Name,
                Salary = e.Salary,
                CurrentPositionNavigation = new Position { Name = e.CurrentPositionNavigation.Name },
                LstPositionHistoryEmployee = e.LstPositionHistoryEmployee
            });
            return await queryEmployees.FirstOrDefaultAsync();
        }
        /* Función que crea un empleado */
        public async Task<(bool success, string result)> Create(Employee employee, DateTime startDate, DateTime? endDate)
        {
            if (await ExistsAsync(employee.IdNum))
                return (false, $"Ya existe un empleado con el número de identificación: {employee.IdNum}");
            await _context.Employees.AddAsync(employee);
            await InsertPositionHistory(employee, startDate, endDate);
            return (true, string.Empty);
        }
        /* Función que actualiza un empleado */
        public async Task<(bool success, string result)> Update(Employee employee, DateTime startDate, DateTime? endDate)
        {
            var getEmployee = await GetByIdTracking(employee.IdNum);
            if (getEmployee == null)
                return (false, $"El empleado con identificación: {employee.IdNum} no existe");

            getEmployee.Name = employee.Name;
            getEmployee.Salary = employee.Salary;
            getEmployee.CurrentPosition = endDate != null ? null : employee.CurrentPosition;

            getEmployee.AuditInfo.ModifiedAt = DateTime.Now;
            getEmployee.AuditInfo.ModifiedBy = 1;

            _context.Employees.Update(getEmployee);
            await InsertPositionHistory(getEmployee, startDate, endDate, false);
            return (true, string.Empty);
        }
        /* Función que elimina un empleado por Identificación */
        public async Task<(bool success, string result)> Delete(string idNum)
        {
            var deleteEmployee = await _context.Employees.Include(x => x.LstPositionHistoryEmployee).Where(x => x.IdNum.Equals(idNum)).FirstOrDefaultAsync();
            if (deleteEmployee != null)
            {
                _context.PositionHistories.RemoveRange(deleteEmployee.LstPositionHistoryEmployee);
                _context.Employees.Remove(deleteEmployee);
                return (true, string.Empty);
            }
            return (false, "El empleado no existe");
        }
        /* Busca todos los empleados que forman parte de un departamento específico y están trabajando en al menos un proyecto */
        public async Task<List<Employee>> GetEmployeesByDepartmentAndProjectQuery(string code)
        {
            // Iniciamos la consulta con AsNoTracking para mejorar el rendimiento en lecturas
            var queryEmployees = _context.Employees.Include(x => x.CurrentPositionNavigation).ThenInclude(x => x.DepartmentNavigation).Include(x => x.LstEmployeeEmployeeProject).AsNoTracking();

            /* Están relacionados a un departamento */
            queryEmployees = queryEmployees.Where(x =>
                x.CurrentPositionNavigation != null &&
                x.CurrentPositionNavigation.DepartmentNavigation.Code.Equals(code));

            /* Están trabanado en al menos un proyecto */
            queryEmployees = queryEmployees.Where(x => x.LstEmployeeEmployeeProject.Any(x => x.ProjectNavigation.DateRange.StartDate <= DateTime.Now && (x.ProjectNavigation.DateRange.EndDate == null || x.ProjectNavigation.DateRange.EndDate >= DateTime.Now)));

            // Seleccionamos sólo los datos que necesitamos (proyección)
            queryEmployees = queryEmployees.Select(e => new Employee
            {
                Id = e.Id,
                IdNum = e.IdNum,
                Name = e.Name,
                Salary = e.Salary,
                CurrentPositionNavigation = new Position { Name = e.CurrentPositionNavigation.Name },
                LstPositionHistoryEmployee = e.LstPositionHistoryEmployee,
                LstEmployeeEmployeeProject = e.LstEmployeeEmployeeProject
            });
            return await queryEmployees.OrderBy(x => x.Id).ToListAsync();
        }

        #endregion

        #region Private Methods
        private async Task<bool> ExistsAsync(string idNum) => await _context.Employees.AsNoTracking().AnyAsync(x => x.IdNum.Equals(idNum));
        private async Task<Employee> GetByIdTracking(string idNum) => await _context.Employees.Where(x => x.IdNum.Equals(idNum)).FirstOrDefaultAsync();

        private async Task<bool> InsertPositionHistory(Employee employee, DateTime startDate, DateTime? endDate, bool isNew = true)
        {
            PositionHistory newPositionHistory = new PositionHistory
            {
                PositionId = employee.CurrentPosition != null ? (int)employee.CurrentPosition : null,
                EmployeeId = isNew ? _context.Entry(employee).Property(e => e.Id).CurrentValue : employee.Id,
                DateRange = new Domain.ValueObjects.DateRange(startDate, endDate),
                Salary = employee.Salary,
                AuditInfo = new Domain.ValueObjects.AuditInfo(isNew ? DateTime.Now : null, isNew ? employee.AuditInfo.CreatedBy : null, !isNew ? DateTime.Now : null, !isNew ? employee.AuditInfo.CreatedBy : null, true)
            };
            await _context.PositionHistories.AddAsync(newPositionHistory);
            return true;
        }


        #endregion
    }
}


