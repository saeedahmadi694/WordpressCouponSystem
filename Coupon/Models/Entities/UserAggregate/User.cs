namespace Coupon.Models.Entities.UserAggregate;

public class User
{
    public int Id { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string UniqueCode { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime? LastLoginTime { get; set; }
    public bool IsActive { get; set; }
}
