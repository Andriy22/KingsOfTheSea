using Application.Common.DTOs.Registration;
using Application.Interfaces;
using Application.Stats.Queries.GetPlayerStatsQuery;
using Application.User.Queries.GetTopPlayersQuery;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;
    private readonly IRazorRenderService _razorRenderService;
    private readonly UserManager<Player> _userManager;

    public AccountController(IRazorRenderService razorRenderService, IEmailService emailService,
        IAccountService accountService, UserManager<Player> userManager, IMediator mediator)
    {
        _razorRenderService = razorRenderService;
        _emailService = emailService;
        _accountService = accountService;
        _userManager = userManager;
        _mediator = mediator;
    }

    [HttpGet("get-top-players")]
    [Authorize]
    public async Task<IActionResult> GetTopPlayersAsync()
    {
        var players = await _mediator.Send(new GetTopPlayersQuery { Limit = 10 });
        return Ok(players);
    }

    [HttpGet("get-profile-stats")]
    [Authorize]
    public async Task<IActionResult> GetProfileStatsAsync()
    {
        var stats = await _mediator.Send(new GetPlayerStatsQuery { PlayerId = User.Identity.Name });

        var user = await _userManager.FindByIdAsync(User.Identity.Name);

        var winrate = 0;

        if (stats.Count > 0) winrate = stats.Count(x => x.IsWin) / stats.Count() * 100;

        return Ok(new
        {
            eloRating = user.EloRating,
            winrate,
            nickname = user.Nickname,
            battles = stats.Count(),
            wins = stats.Count(x => x.IsWin),
            losses = stats.Count(x => !x.IsWin),
            registration = user.CreationTime.ToShortDateString(),
            stats
        });
    }

    [HttpGet("confirm-email/{userId}/{code}")]
    public async Task<ContentResult> ConfirmEmail(string userId, string code)
    {
        await _accountService.ConfirmEmailAsync(userId, code);

        var result = await _razorRenderService.RenderEmailConfirmedViewAsync();

        return new ContentResult
        {
            Content = result,
            ContentType = "text/html"
        };
    }


    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccountAsync([FromBody]RegistrationDTO model)
    {
        await _accountService.RegistrationAsync(model);

        // var url = await _accountService.GenerateEmailConfirmationLinkAsync(model.Email, @"https://localhost:7196/api/account/confirm-email");

        //await _emailService.SendEmailAsync(model.Email, "WoT-STATS Email Confirmation", await _razorRenderService.RenderEmailConfirmationAsync(new Application.ViewModels.EmailConfirmationVM()
        //{
        //    ConfirmationLink = url,
        //    Username = "null"
        //}));

        return Ok();
    }
}