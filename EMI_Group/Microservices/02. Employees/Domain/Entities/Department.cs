using Domain.Primitives;
using Domain.ValueObjects;
namespace Domain.Entities;
public class Department : AggregateRoot
{
    public Department()
    {
        AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
        LstDepartmentPositions = new HashSet<Position>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public AuditInfo AuditInfo { get; set; }
    public ICollection<Position> LstDepartmentPositions { get; set; }
}
