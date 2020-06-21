using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YourBudget.API.Models;
using YourBudget.API.Services.ViewModels;
using YourBudget.API.Utils;

namespace YourBudget.API.Services
{
    public class BudgetService : BaseService
    {
        public BudgetService(BudgetContext context, Localizer localizer) : base(context, localizer)
        {
        }

        public Result<BudgetView> GetBudget(Guid userId)
        {
            var dao = _context.Budgets.FirstOrDefault(item => item.Owner.Id == userId);

            if (dao == null)
                dao = _context.BudgetUsers.FirstOrDefault(item => item.UserId == userId && item.Allowed)?.Budget;

            if (dao == null)
                return Result<BudgetView>.Error(_t["Бюджет не найден"]);

            var view = new BudgetView(dao);
            return Result<BudgetView>.Success(view);
        }

        public Result<BudgetView> CreateBudget(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(item => item.Id == userId);
            if (user == null)
                return Result<BudgetView>.Error(_t["Пользователь не найден"]);

            if (_context.Budgets.Any(item => item.Owner.Id == userId))
                return Result<BudgetView>.Error(_t["У пользователя уже есть бюджет"]);

            var budget = new Budget
            {
                Owner = user,
                Opened = DateTime.UtcNow
            };

            _context.Budgets.Add(budget);
            _context.SaveChanges();

            return Result<BudgetView>.Success(new BudgetView(budget));
        }

        public Result<IEnumerable<OperationView>> GetOperations(Guid budgetItemId)
        {
            var operations = _context.Operations.Where(item => item.BudgetItem.Id == budgetItemId).Select(item => new OperationView(item)).ToList();
            return Result<IEnumerable<OperationView>>.Success(operations);
        }

        public Result<IEnumerable<BudgetItemView>> CreateBudgetItem(Guid budgetId, BudgetItemView newItem)
        {
            var budget = _context.Budgets.FirstOrDefault(item => item.Id == budgetId);
            if (budget == null)
                return Result<IEnumerable<BudgetItemView>>.Error(_t["Бюджет не найден"]);

            if (newItem == null)
                return Result<IEnumerable<BudgetItemView>>.Error(_t["Раздел буджета не может быть пустым"]);

            var dao = new BudgetItem
            {
                Budget = budget,
                Credit = 0.0,
                Debet = newItem.Debet,
                Name = newItem.Name,
                Order = newItem.Order
            };

            _context.BudgetItems.Add(dao);
            _context.SaveChanges();
            var items = _context.BudgetItems.Where(item => item.Budget.Id == budgetId).Select(item => new BudgetItemView(item)).ToList();
            return Result<IEnumerable<BudgetItemView>>.Success(items);
        }

        public Result<BudgetItemView> UpdateBudgetItem(BudgetItemView budgetItem)
        {
            if (budgetItem == null)
                return Result<BudgetItemView>.Error(_t.EmptyFieldMsg);

            var dao = _context.BudgetItems.FirstOrDefault(item => item.Id == budgetItem.Id);
            dao.Name = budgetItem.Name;
            dao.Order = budgetItem.Order;
            dao.Planned = budgetItem.Planned;
            dao.Сumulative = budgetItem.Сumulative;
            _context.SaveChanges();
            return Result<BudgetItemView>.Success(new BudgetItemView(dao));
        }

        public Result<BudgetView> CommitOperation(Guid userId, Guid budgetItemId, OperationView operation)
        {
            var user = _context.Users.FirstOrDefault(item => item.Id == userId);
            if (user == null)
                return Result<BudgetView>.Error(_t["Пользовател не найден"]);

            var budgetItem = _context.BudgetItems.FirstOrDefault(item => item.Id == budgetItemId);
            if (budgetItem == null)
                return Result<BudgetView>.Error(_t["Раздел буджета не найден"]);

            if (operation == null)
                return Result<BudgetView>.Error(_t["Информация об операции не может быть пустой"]);

            var budget = budgetItem.Budget;

            var dao = new Operation
            {
                Actor = user,
                Amount = operation.Amount,
                BudgetItem = budgetItem,
                Category = operation.Category,
                Discription = operation.Discription,
                Type = operation.Type
            };

            using (var transaction = new TransactionScope())
            {
                _context.Operations.Add(dao);
                switch (operation.Type)
                {
                    case OperationType.Debet:
                        budgetItem.Debet += operation.Amount;
                        budget.Balance += operation.Amount;
                        break;
                    case OperationType.Credit:
                        budgetItem.Credit += operation.Amount;
                        budget.Balance -= operation.Amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("operation.Type");
                }                
                _context.SaveChanges();
                transaction.Complete();
            }
            
            return Result<BudgetView>.Success(new BudgetView(budget));
        }

        public Result<BudgetView> CloseBudget(Guid budgetId)
        {
            var dao = _context.Budgets.FirstOrDefault(item => item.Id == budgetId);
            if (dao == null)
                return Result<BudgetView>.Error(_t["Бюджет не найден"]);

            using (var transaction = new TransactionScope())
            {
                foreach (var item in dao.Items)
                {
                    var current = item.Planned + item.Debet - item.Credit;
                    if (item.Сumulative)
                        item.Planned += current;

                    item.Debet = 0;
                    item.Credit = 0;
                }

                dao.Opened = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return Result<BudgetView>.Success(new BudgetView(dao));
        }
    }

}
