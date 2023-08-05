using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MorningStar.Extension
{
    /// <summary>
    /// Authorization 容器服务
    /// </summary>
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            var secretKey = AppSettings.Get("Jwt:SecretKey");
            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Production"))
                secretKey = Environment.GetEnvironmentVariable("JWT_SECRETKEY") ?? string.Empty;
            if (string.IsNullOrEmpty(secretKey)) throw new Exception("容器服务：【Authorization】注册错误：secretKey为空！");
            //Console.WriteLine(secretKey);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AppSettings.Get("Jwt:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = AppSettings.Get("Jwt:Audience"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    RequireExpirationTime = true,
                };
            });

            Console.WriteLine("容器服务：【Authorization】已注册！");
        }
    }
}
