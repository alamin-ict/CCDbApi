using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CCDbApi
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {

            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
              var endpoint = context.GetEndpoint();

                //Skip authentication if the endpoint has[AllowAnonymous] attribute
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    await _next(context);
                    return;
                }
                var token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    AttachUserToContext(context, token);

                }
                // Check if the user is authenticated
                if (context.User?.Identity?.IsAuthenticated != true)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("User not authenticated.");
                    return; // Exit to prevent further processing
                }

                // Proceed to the next middleware in the pipeline

                await _next(context);
           

        }




        private void AttachUserToContext(HttpContext context, string token)
        {

           
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:AppSecret"]);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    // Safely retrieve claims
                    var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == "Email")?.Value
                              ?? jwtToken.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                                

                    if (userId != null || username != null)
                    {
                        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;

                    }
                }
                catch (Exception ex)
                {
                    //Return an Unauthorized response with the error message
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.WriteAsync($"Token validation failed: {ex.Message}");
                }
            }
            
            
        
        //private void AttachUserToContext(HttpContext context, string token)
        //{
        //    try
        //    {
        //        var appSecret = _configuration["Jwt:AppSecret"];
        //        var issuer = _configuration["Jwt:Issuer"];
        //        var audience = _configuration["Jwt:Audience"];

        //        if (string.IsNullOrWhiteSpace(appSecret) || appSecret.Length < 32)
        //            throw new InvalidOperationException("Invalid or missing JWT secret key in configuration.");

        //        var key = Encoding.UTF8.GetBytes(appSecret);
        //        var tokenHandler = new JwtSecurityTokenHandler();

        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),

        //            ValidateIssuer = true,
        //            ValidIssuer = issuer,

        //            ValidateAudience = true,
        //            ValidAudience = audience,

        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero,

        //            RequireSignedTokens = true,
        //            ValidateTokenReplay = false,
        //            ValidateActor = false
        //        };

        //        tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        //        var jwtToken = validatedToken as JwtSecurityToken;

        //        if (jwtToken == null || jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
        //            throw new SecurityTokenException("Invalid token algorithm.");

        //        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        //        var principal = new ClaimsPrincipal(identity);
        //        context.User = principal;
        //    }
        //    catch (SecurityTokenException ex)
        //    {
        //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //        context.Response.ContentType = "text/plain";
        //        context.Response.WriteAsync($"Token validation failed: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //        context.Response.ContentType = "text/plain";
        //        context.Response.WriteAsync($"Internal server error: {ex.Message}");
        //    }
        //}

    }
}
