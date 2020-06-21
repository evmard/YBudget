using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourBudget.API.Models
{
    public class Budget : EntityBase
    {
        [Required]
        public User Owner { get; set; }

        public List<BudgetUser> Users { get; set; }

        [Required]
        public double Balance { get; set; }
        public List<BudgetItem> Items { get; set; }
        public DateTime Opened { get; set; }
    }
}
