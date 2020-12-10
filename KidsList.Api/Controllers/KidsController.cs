using System.Threading.Tasks;
using KidsList.Services.Kids;
using KidsList.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidsList.Api.Controllers
{
    [Route("api/kids")]
    [ApiController]
    public class KidsController : BaseApiController
    {
        private readonly IKidService _kidService;

        public KidsController(IUserService userService, IKidService kidService) : base(userService)
        {
            _kidService = kidService;
        }

        // POST: api/Kids
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<KidDto>> AddKid(AddKidRequest request)
        {
            var kidDto = await _kidService.AddKidToFamily(CurrentUser.Id, request);
            if (kidDto == null)
            {
                // TODO, will this happen?
                return NotFound();
            }
            return kidDto;
        }
    }
}
