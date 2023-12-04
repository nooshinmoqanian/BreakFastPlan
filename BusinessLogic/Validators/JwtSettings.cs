
namespace BusinessLogic.Validators
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string KeyAccess { get; set; } = string.Empty;
        public string KeyRefresh { get; set; } = string.Empty;
    }
}
