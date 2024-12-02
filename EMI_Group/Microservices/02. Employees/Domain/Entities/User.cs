using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;
public class User : AggregateRoot
{
    public User()
    {
        AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true);
    }
    public User(string username, string passwordHash, int createdBy)
    {
        Username = username;
        PasswordHash = passwordHash;
        AuditInfo = new AuditInfo(DateTime.Now, createdBy, null, null, true);
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public int UserRole { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiryDate { get; set; }
    public UserRole UserRoleNavigation { get; set; }
    public AuditInfo AuditInfo { get; set; }
}
