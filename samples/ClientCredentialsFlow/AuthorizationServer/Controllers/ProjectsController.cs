using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenIddict.Core;
using OpenIddict.Models;

namespace AuthorizationServer.Controllers
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        private IProjectRepository _projectRepository;

        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;

        public ProjectsController(OpenIddictApplicationManager<OpenIddictApplication> applicationManager, IProjectRepository projectRepository)
        {
            _applicationManager = applicationManager;
            _projectRepository = projectRepository;
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("getprojectlistasync")]
        public async Task<IActionResult> GetProjectListAsync( int Id_Profile)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }
            
            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            var response = await _projectRepository.GetAllAsync(Id_Profile);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("getprojectbound")]
        public async Task<IActionResult> Getprojectbound(int Id_Profile)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            var response = await _projectRepository.Getprojectbound(Id_Profile);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("gobound")]
        public async Task<IActionResult> GoBound(string name,int id_project)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            await _projectRepository.GoBound(name, id_project);

            return Ok(201);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("goboundprofile")]
        public async Task<IActionResult> GoBoundProfile(int id_project)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

           var rewponse = await _projectRepository.GoBoundProfile(id_project);

            return Ok(rewponse);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("getprojectlistarhiveasync")]
        public async Task<IActionResult> GetProjectListArhiveAsync(int Id_Profile)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            var response = await _projectRepository.GetAllArhiveAsync(Id_Profile);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("getone")]
        public async Task<IActionResult> GetOne(int Id)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            var response = await _projectRepository.GetOneAsync(Id);

            return Ok(response);
        } //!!!!!!!!!!!!!!!!!!!!!!!

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("returnfromarchive")]
        public async Task<IActionResult> ReturnFromArchive(int Id)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            await _projectRepository.ReturnFromArchive(Id);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("editproject")]
        public async Task<IActionResult> EditProject(Project requaet)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            Project project = new Project()
            {
                Id = requaet.Id,
                Title = requaet.Title,
                Description = requaet.Description
            };

            await _projectRepository.EditProject(project);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("goarhive")]
        public async Task<IActionResult> GoArhive(int Id)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            await _projectRepository.GoArhive(Id);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("createproject")]
        public async Task<IActionResult> CreateProject(Project requaet)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            Project project = new Project()
            {
                Title = requaet.Title,
                Description = requaet.Description,
                Arhive = "False",
                Id_Profile = requaet.Id_Profile
            };

         var response = await _projectRepository.CreateProject(project);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("setprojecttodoist")]
        public async Task<IActionResult> SetProjectTodoistAsync(Project requaet)
        {
            var subject = User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest();
            }

            var application = await _applicationManager.FindByClientIdAsync(subject, HttpContext.RequestAborted);
            if (application == null)
            {
                return BadRequest();
            }

            Project project = new Project()
            {
                Title = requaet.Title,
                Arhive = "False",
                Description = "",
                Id_Profile = requaet.Id_Profile,
            };

            var response = await _projectRepository.CreateProject(project);

            return Ok(response);
        }
    }
}