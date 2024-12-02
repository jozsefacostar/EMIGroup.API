using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Position : AggregateRoot
    {
        public Position()
        {
            LstPositionEmployees = new HashSet<Employee>();
            LstPositionHistoryPosition = new HashSet<PositionHistory>();
            AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool RegularJob { get; set; }
        public int Department { get; set; }
        public Department DepartmentNavigation { get; set; }
        public virtual ICollection<Employee> LstPositionEmployees { get; set; }
        public virtual ICollection<PositionHistory> LstPositionHistoryPosition { get; set; }
        public AuditInfo AuditInfo { get; set; }
    }
}
