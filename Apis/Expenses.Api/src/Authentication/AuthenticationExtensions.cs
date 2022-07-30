using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Api.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            string authority,
            string audience
            )
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(60)
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.Authority = authority;
                x.Audience = audience;
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = validationParameters;
                //x.SecurityTokenValidators.Clear();
                //x.SecurityTokenValidators.Add(new GoogleJwtTokenValidator(audience));
            });
            

            return services;
        }
    }
}
