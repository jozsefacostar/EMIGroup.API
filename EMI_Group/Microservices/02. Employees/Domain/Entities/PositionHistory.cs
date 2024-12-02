using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class PositionHistory : AggregateRoot
    {
        public PositionHistory()
        {
            AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? PositionId { get; set; }
        public decimal Salary{ get; set; }
        public required DateRange DateRange { get; set; }
        public Employee EmployeeNavigation { get; set; }
        public Position PositionNavigation { get; set; }
        public AuditInfo AuditInfo { get; set; }
    }
}
