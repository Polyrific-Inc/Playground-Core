using System.ComponentModel.DataAnnotations;

namespace PG.Api.Domains.Account
{
    public class CredentialDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
