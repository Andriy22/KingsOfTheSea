﻿using System.Text;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Persistence;

public static class DependencyInjection
{
    private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        if (expires != null) return DateTime.UtcNow < expires;
        return false;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection
        services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
            {
                b.EnableRetryOnFailure();
                b.MigrationsAssembly("API");
            }));


        // identity
        services.AddDefaultIdentity<Player>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();

        var builder = services.AddIdentityCore<Player>(o =>
        {
            // configure identity options
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 6;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

        var KEY = configuration.GetSection("JWT").GetValue<string>("KEY");
        var SSK = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("JWT").GetValue<string>("ISSUER"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("JWT").GetValue<string>("AUDIENCE"),
                    ValidateLifetime = true,
                    LifetimeValidator = CustomLifetimeValidator,
                    IssuerSigningKey = SSK,
                    ValidateIssuerSigningKey = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/api/live"))
                        {
                            var token = accessToken.ToString();

                            context.Token = token.Split(' ').LastOrDefault();
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<IDataContext, DataContext>();

        return services;
    }
}