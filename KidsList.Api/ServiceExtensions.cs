using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using KidsList.Data;
using KidsList.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Auth0.AuthenticationApi;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using KidsList.AuthService;
using KidsList.Services.Kids;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void AddPostgres(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringKey)
        {
            services.AddDbContext<KidsListContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(connectionStringKey))
                .UseSnakeCaseNamingConvention());
        }

        public static void AddKidsListServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKidService, KidService>();
        }

        public static void AddAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            var domain = $"https://{configuration["Auth0:Domain"]}/";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = configuration["Auth0:Audience"];
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken token)
                        {
                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", token.RawData));
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Email
                };
            });

            services.AddSingleton<IAuthenticationApiClient>(sp =>
                new AuthenticationApiClient(new Uri($"https://{configuration["Auth0:Domain"]}/")));

            services.AddSingleton<IAuthService, AuthService>();
        }

        public static void AddCustomCors(this IServiceCollection services, string policyName, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder
                        .WithOrigins(configuration.GetSection("Frontend").Get<string[]>())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KidsList API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization Header",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
