using System;
using YourBudget.API.Models;

namespace YourBudget.API.Services.ViewModels
{
    public class BudgetItemView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public double Debet { get; set; }
        public double Credit { get; set; }
        public double Planned { get; set; }
        public bool Сumulative { get; set; }

        public double CurrentAmount
        {
            get
            {
                return Planned + Debet - Credit;
            }
        }

        public BudgetItemView(BudgetItem item)
        {
            Id = item.Id;
            Name = item.Name;
            Order = item.Order;
            Debet = item.Debet;
            Credit = item.Credit;
            Planned = item.Planned;
            Сumulative = item.Сumulative;
        }
    }
}