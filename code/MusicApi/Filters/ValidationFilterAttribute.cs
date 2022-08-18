using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MusicApi.Filters
{
  // NOTE: Validation using filter attribute.
  public class ValidationFilterAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        context.Result = new UnprocessableEntityObjectResult(context.ModelState);
      }
    }

    public override void OnActionExecuted(ActionExecutedContext context) { }
  }
}