using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenIddict.Core;
using OpenIddict.Models;

namespace AuthorizationServer.Controllers
{
    [EnableCors("MyPolicy")]
    public class AuthorizationController : Controller
    { 
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;

        private readonly IProfileRepository pRepository;

        public AuthorizationController(OpenIddictApplicationManager<OpenIddictApplication> applicationManager, IProfileRepository profileRepository)
        {
            _applicationManager = applicationManager;
            pRepository = profileRepository;
        }

        [AllowAnonymous]
        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Login(OpenIdConnectRequest request,Profile profile)
        {
            if (request.IsClientCredentialsGrantType())
            {
                var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);
                if (application == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidClient,
                        ErrorDescription = "Id Клиента не найден в базе данных."
                    });
                }

                if (! await pRepository.GetProfileAsync(profile.Login, profile.Password))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                        ErrorDescription = "Login or PAssword не совпадают."
                    });
                }
                var idprofile = await pRepository.GetProfileIdAsync(profile.Login, profile.Password);
                var b = idprofile.ToString();
                request.Username = b.ToString();
                var ticket = CreateTicket(request, application);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            
            else if (request.IsRefreshTokenGrantType())
            {
                var info = await HttpContext.AuthenticateAsync(OpenIdConnectServerDefaults.AuthenticationScheme);


                var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);
                if (application == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidClient,
                        ErrorDescription = "Id Клиента не найден в базе данных."
                    });
                }
                 var ticket = CreateTicket(request, application);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });

        }

        private AuthenticationTicket CreateTicket(OpenIdConnectRequest request, OpenIddictApplication application)
        {
            var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);
            
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject, application.ClientId,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);


            identity.AddClaim(OpenIdConnectConstants.Claims.Name, request.Username,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);
            
            if (!request.IsRefreshTokenGrantType())
            {
                ticket.SetScopes(OpenIdConnectConstants.Scopes.OfflineAccess);
            }

            ticket.SetResources("resource_server"); //если обслуживают несколько серверов

            return ticket;
        }
    }
}