using DataSource;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccreditationAPI.Filters
{
    public class UnitOfWorkFilter : IActionFilter
    {
        readonly IUnitOfWork unitOfWork;

        public UnitOfWorkFilter(IUnitOfWork unitOfWork)
        => this.unitOfWork = unitOfWork;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        => unitOfWork.SaveChanges();
    }
}
