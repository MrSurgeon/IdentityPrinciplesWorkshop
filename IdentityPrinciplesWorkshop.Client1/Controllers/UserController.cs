using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityPrinciplesWorkshop.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IConfiguration Configuration { get; }
        public UserController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
           
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
        public async Task<IActionResult> RefreshTokens()
        {
            HttpClient httpClient = new HttpClient();
            var discoDoc = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (discoDoc.IsError)
            {
                //loglama yap
                return BadRequest();
            }

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var tokenList= await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                ClientId = Configuration["Client:clientId"],
                ClientSecret = Configuration["Client:clientSecret"],
                RefreshToken = refreshToken,
                Address = discoDoc.TokenEndpoint
            });

            if (tokenList.IsError)
            {
                return BadRequest();
            }

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken()
                {Name = "id_token",Value = tokenList.IdentityToken},
                new AuthenticationToken(){Name = "access_token",Value = tokenList.AccessToken},
                new AuthenticationToken(){Name = "refresh_token",Value = tokenList.RefreshToken},
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(tokenList.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)
                }
            };
            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }
        public IActionResult Signout()
        {
            return SignOut("Cookies", "oidc");
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [Authorize(Roles = "admin,customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
