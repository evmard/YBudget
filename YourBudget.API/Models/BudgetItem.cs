using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourBudget.API.Models
{
    public class BudgetItem : EntityBase
    {
        [Required]
        public Guid BudgetId { get; set; }
        public Budget Budget { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public bool Сumulative { get; set; }

        public double Planned { get; set; }

        public double Debet { get; set; }

        public double Credit { get; set; }

        public List<Operation> Operations { get; set; }
    }
}