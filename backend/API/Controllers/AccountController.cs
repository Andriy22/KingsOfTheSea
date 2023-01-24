using Application.Interfaces;
using Application.Language.Queries.GetUserLanguageQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IRazorRenderService razorRenderService;

        public AccountController(IRazorRenderService razorRenderService)
        {
            this.razorRenderService = razorRenderService;
        }

        [HttpGet("get-user-language")]
        public async Task<IActionResult> GetUserLanguageAsync()
        {
            return Ok(await Mediator.Send(new GetUserLanguageQuery { UserId = UserId }));
        }
    }
}
