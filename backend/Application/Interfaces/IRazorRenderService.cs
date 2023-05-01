using Application.ViewModels;

namespace Application.Interfaces;

public interface IRazorRenderService
{
    public Task<string> RenderEmailConfirmationAsync(EmailConfirmationVM model);
    public Task<string> RenderEmailConfirmedViewAsync();
}