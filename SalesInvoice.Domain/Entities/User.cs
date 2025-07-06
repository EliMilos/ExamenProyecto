
namespace SalesInvoice.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Cedula { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public bool IsLocked { get; set; } = false;
    public int FailedLoginAttempts { get; set; } = 0;

    public byte[]? RowVersion { get; set; } // Concurrency token

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}


