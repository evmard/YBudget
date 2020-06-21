using Microsoft.EntityFrameworkCore;

namespace YourBudget.API.Models
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<BudgetUser> BudgetUsers { get; set; }
    }
}
