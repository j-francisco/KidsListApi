using System.Linq;
using System.Threading.Tasks;
using KidsList.Services.Users;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KidsList.Api.ActionFilters
{
    public class CurrentUserActionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public CurrentUserActionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // The Type value is configured in Auth0.
            // TODO move to app setting.
            var userClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "https://kidslist-dev/api/email");

            if (userClaim == null)
            {
                return;
            }

            var currentUserEmail = userClaim.Value;

            var user = await _userService.GetUser(currentUserEmail);

            if (user != null)
            {
                context.HttpContext.Items["CurrentUser"] = user;
            }

            await next();
        }
    }
}
