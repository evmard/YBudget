using YourBudget.API.Models;
using YourBudget.API.Utils;

namespace YourBudget.API.Services
{
    public abstract class BaseService
    {
        protected readonly BudgetContext _context;
        protected readonly Localizer _t;

        protected BaseService(BudgetContext context, Localizer localizer)
        {
            _context = context;
            _t = localizer;
        }
    }
}
