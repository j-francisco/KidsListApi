using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KidsList.Services.Users;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using KidsList.AuthService;

namespace KidsList.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService) : base(userService)
        {
            _authService = authService;
        }

        // GET: api/Users/current
        // TODO since this creates new users, maybe it should be a POST?
        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // This calls directly to the context rather than CurrentUser in BaseApiController,
            // because we don't want to throw an exception if not found.
            // Instead, we'll create a new account if not found.
            if (HttpContext.Items["CurrentUser"] is UserDto currentUser)
            {
                return currentUser;
            }

            // Try to sign up a new user
            var accessTokenClaim = User.Claims.FirstOrDefault(c => c.Type == "access_token");

            if (string.IsNullOrEmpty(accessTokenClaim?.Value))
            {
                return NotFound();
            }

            // Get user info from auth service
            var authUser = await _authService.GetUserInfo(accessTokenClaim.Value);

            if (authUser?.Email == null)
            {
                return NotFound();
            }

            var createUserRequest = new CreateUserRequest
            {
                Email = authUser.Email,
                FullName = authUser.FullName
            };

            return await UserService.CreateUserAndFamily(createUserRequest);
        }

        // // PUT: api/Users/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutUser(int id, User user)
        // {
        //     if (id != user.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(user).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!UserExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        
    }
}
