using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace YourBudget.API.Models
{
    public class User : EntityBase
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [MaxLength(200)]
        public string PassHash { get; set; }

        [MaxLength(100)]
        public string ResetToken { get; set; }

        [MaxLength(100)]
        public string Mail { get; set; }

        [MaxLength(100)]
        public string Language { get; set; }
    }
}
