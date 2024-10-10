using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bills.Api.Filters
{
    public class InjectUserIdFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst("UserId");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                context.Result = new UnauthorizedObjectResult("Invalid Token!");
                return;
            }

            if (userIdClaim != null)
            {
                var userId = Convert.ToInt32(userIdClaim.Value);

                foreach (var argument in context.ActionArguments.Values)
                {
                    var userIdProperty = argument?.GetType().GetProperty("userId");
                    if (userIdProperty != null && userIdProperty.PropertyType == typeof(int?))
                    {
                        userIdProperty.SetValue(argument, userId);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
