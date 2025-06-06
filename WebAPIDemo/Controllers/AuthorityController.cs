﻿using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPIDemo.Authority;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    public class AuthorityController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public AuthorityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AppCredential credential)
        {

            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(1);

                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, Configuration.GetValue<string>("SecretKey")),
                    expires_at = expiresAt
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }

        }

        

    }
}
