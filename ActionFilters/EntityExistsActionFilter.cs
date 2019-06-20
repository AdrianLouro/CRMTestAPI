using System;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Entities.Contracts;

namespace ActionFilters
{
    public class EntityExistsActionFilter<T> : IActionFilter where T : class, IEntity
    {
        private readonly AppDbContext _dbContext;

        public EntityExistsActionFilter(AppDbContext context)
        {
            _dbContext = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Guid id = Guid.Empty;
            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (Guid) context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }

            var entity = _dbContext.Set<T>().SingleOrDefault(
                x =>
                    x is IRemoveAware
                        ? x.Id.Equals(id) && !((IRemoveAware) x).IsDeleted()
                        : x.Id.Equals(id)
            );

            if (entity == null)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("entity", entity);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}