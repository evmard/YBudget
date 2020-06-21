using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBudget.API.Models;

namespace YourBudget.API.Services.ViewModels
{
    public class BudgetView
    {
        public Guid Id { get; set; }
        public double Balance { get; set; }
        public IEnumerable<BudgetItemView> Items { get; set; }
        public DateTime Opened { get; set; }

        public bool NeedToClose
        {
            get
            {
                var nextMounth = new DateTime(Opened.Year, Opened.Month, 1).AddMonths(1);
                return nextMounth <= DateTime.UtcNow;
            }
        }

        public BudgetView(Budget dao)
        {
            Id = dao.Id;
            Balance = dao.Balance;
            Items = dao.Items?.Select(item => new BudgetItemView(item)).ToList();
            Opened = dao.Opened;
        }
    }
}
