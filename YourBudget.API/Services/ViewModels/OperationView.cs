using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBudget.API.Models;

namespace YourBudget.API.Services.ViewModels
{
    public class OperationView
    {
        public string ActorName { get; set; }
        public OperationType Type { get; set; }
        public double Amount { get; set; }
        public string Discription { get; set; }
        public string Category { get; set; }
        public DateTime Commited { get; set; }

        public OperationView(Operation item)
        {
            ActorName = item.Actor.Name;
            Type = item.Type;
            Amount = item.Amount;
            Discription = item.Discription;
            Category = item.Category;
            Commited = item.Commited;
        }
    }
}
