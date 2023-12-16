
namespace DataAccess.Models
{
    public class Users 
    {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
        }
    }

