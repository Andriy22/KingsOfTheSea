using Application.Common.DTOs.Registration;

namespace Application.Interfaces;

public interface IAccountService
{
    Task RegistrationAsync(RegistrationDTO model);
    Task<string> GenerateEmailConfirmationLinkAsync(string email, string callbackUrl);
    Task ConfirmEmailAsync(string userId, string code);
}