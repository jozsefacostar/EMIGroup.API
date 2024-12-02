using Domain.Primitives;
using Domain.ValueObjects;
namespace Domain.Entities;
public class EmployeeProject : AggregateRoot
{
    public EmployeeProject(int employee, int project)
    {
        Employee = employee;
        Project = project;
        AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
    }
    public int Id { get; set; }
    public int Employee { get; set; }
    public int Project { get; set; }
    public Employee EmployeeNavigation { get; set; }
    public Project ProjectNavigation { get; set; }
    public AuditInfo AuditInfo { get; set; }
}
