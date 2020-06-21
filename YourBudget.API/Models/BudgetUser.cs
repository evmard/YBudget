using System;
using System.ComponentModel.DataAnnotations;

namespace YourBudget.API.Models
{
    public class BudgetUser : EntityBase
    {
        [Required]
        public Guid BudgetId { get; set; }
        public Budget Budget { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool Allowed { get; set; }
    }
}
