namespace Domain.ValueObjects;
public class AuditInfo
{
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public bool Active { get; set; }

    public AuditInfo(DateTime? createdAt, int? createdBy, DateTime? modifiedAt, int? modifiedBy, bool active)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        ModifiedAt = modifiedAt;
        ModifiedBy = modifiedBy;
        Active = active;
    }
    public bool IsActive() => Active;
    public void SetActive(bool active) => Active = active;
}