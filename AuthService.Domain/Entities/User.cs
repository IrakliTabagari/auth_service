namespace AuthService.Domain.Entities;

public class User : BaseEntity<int>
{
    // string firstName, string lastName, string email, string password
    public string FirstName  { get; set; }
    public string LastName  { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    
}