using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Employee : AggregateRoot
    {
        public Employee()
        {
            AuditInfo = new AuditInfo(DateTime.Now, 1, null, null, true);
            LstPositionHistoryEmployee = new HashSet<PositionHistory>();
            LstEmployeeEmployeeProject = new HashSet<EmployeeProject>();
        }
        public Employee(string idNum, string name, int position, decimal salary, int? CreatedBy, int? ModifiedBy)
        {
            IdNum = idNum;
            Name = name;
            CurrentPosition = position;
            Salary = salary;
            AuditInfo = new AuditInfo(CreatedBy != null ? DateTime.Now: null, CreatedBy, ModifiedBy != null ? DateTime.Now : null, ModifiedBy, true);
        }



        public int Id { get; set; }
        public string IdNum { get; set; }
        public string Name { get; set; }
        public int? CurrentPosition { get; set; }
        public decimal Salary { get; set; }
        public Position CurrentPositionNavigation { get; set; }
        public AuditInfo AuditInfo { get; set; }
        public virtual ICollection<PositionHistory> LstPositionHistoryEmployee { get; set; }
        public virtual ICollection<EmployeeProject> LstEmployeeEmployeeProject { get; set; }



    }
}
