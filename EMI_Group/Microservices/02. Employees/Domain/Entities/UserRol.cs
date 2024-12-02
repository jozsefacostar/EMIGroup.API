using Domain.Entities;
using Domain.Primitives;
using Domain.ValueObjects;

public class UserRole : AggregateRoot
{
    public UserRole()
    {
        AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
        LstUser = new HashSet<User>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public AuditInfo AuditInfo { get; set; }
    public ICollection<User> LstUser { get; set; }
}
