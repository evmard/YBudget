using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourBudget.API.Services;
using YourBudget.API.Services.ViewModels;
using YourBudget.API.Utils;

namespace YourBudget.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private BudgetService _service;

        public BudgetController(BudgetService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Result<BudgetView>> Budget()
        {
            Guid userId = GetUID();
            return _service.GetBudget(userId);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Result<BudgetView>> NewBudget()
        {
            Guid userId = GetUID();
            return _service.CreateBudget(userId);
        }
        
        [Authorize]
        [HttpGet("{budgetItemId}")]
        public ActionResult<Result<IEnumerable<OperationView>>> Operations(Guid budgetItemId)
        {
            //TODO SEC. item owner
            return _service.GetOperations(budgetItemId);
        }

        
        [Authorize]
        [HttpPost("{budgetId}")]
        public ActionResult<Result<IEnumerable<BudgetItemView>>> BudgetItem(Guid budgetId, [FromBody] BudgetItemView newItem)
        {
            return _service.CreateBudgetItem(budgetId, newItem);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<Result<BudgetItemView>> UpdateBudgetItem([FromBody] BudgetItemView budgetItem)
        {
            return _service.UpdateBudgetItem(budgetItem);
        }
        
        [Authorize]
        [HttpPost("{budgetItemId}")]
        public ActionResult<Result<BudgetView>> CommitOperation(Guid budgetItemId, [FromBody] OperationView operation)
        {
            var userId = GetUID();
            return _service.CommitOperation(userId, budgetItemId, operation);
        }

        private Guid GetUID()
        {
            return Guid.Parse(HttpContext.User.Claims.First(item => item.Type == "Guid").Value);
        }
    }
}
