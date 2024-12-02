using Domain.Primitives;
using Domain.ValueObjects;
namespace Domain.Entities;
public class Project : AggregateRoot
{
    public Project()
    {
        AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
        LstProjectEmployeeProject = new HashSet<EmployeeProject>();
        DateRange = new DateRange(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1));
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public required DateRange DateRange { get; set; }
    public AuditInfo AuditInfo { get; set; }
    public virtual ICollection<EmployeeProject> LstProjectEmployeeProject { get; set; }
}
