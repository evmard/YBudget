using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourBudget.API.Models
{
    public class Operation : EntityBase
    {
        [Required]
        public Guid ActorId { get; set; }
        public User Actor { get; set; }

        [Required]
        public OperationType Type { get; set; }

        [Required]
        public Guid BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; }

        [Required]
        public double Amount { get; set; }

        public string Discription { get; set; }

        [MaxLength(250)]
        public string Category { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Commited { get; set; }
    }

    public enum OperationType
    {
        Debet,
        Credit
    }
}