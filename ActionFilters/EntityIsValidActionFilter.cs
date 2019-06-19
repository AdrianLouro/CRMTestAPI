using System.Linq;
using Entities.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilters
{
    public class EntityIsValidActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (GetEntity(context) == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        private object GetEntity(ActionExecutingContext context)
        {
            return context.ActionArguments.SingleOrDefault(argument => argument.Value is IEntityModel).Value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}