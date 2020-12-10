using KidsList.Api.Exceptions;
using KidsList.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace KidsList.Api.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IUserService UserService;

        protected BaseApiController(IUserService userService)
        {
            UserService = userService;
        }

        protected UserDto CurrentUser
        {
            get
            {
                if (HttpContext.Items["CurrentUser"] is UserDto currentUser)
                {
                    return currentUser;
                }

                throw new UserMissingException("A valid access token was provided, but no valid user account was matched. You must call the api/users/current endpoint first.");
            }
        }
    }
}
