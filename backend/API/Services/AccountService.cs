using Application.Common.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        public async Task RegistrationAsync(RegistrationDTO model)
        {
            var result = await userManager.CreateAsync(new AppUser()
            {
                Email = model.Email,
                LastLogIn = DateTime.UtcNow,
                CreationTime = DateTime.UtcNow,
                UserName = model.Email,
                Nickname = model.Username
            }, model.Password);

            if (!result.Succeeded)
            {

            }
        }
    }
}
