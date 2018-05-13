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
using OpenIddict.Core;
using OpenIddict.Models;

namespace AuthorizationServer.Controllers
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/Tasks")]
    public class TasksController : Controller
    {
        private ITaskRepository _taskRepository;

        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;

        public TasksController(OpenIddictApplicationManager<OpenIddictApplication> applicationManager, ITaskRepository taskRepository)
        {
            _applicationManager = applicationManager;
            _taskRepository = taskRepository;
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("gettasks")]
        public async Task<IActionResult> GetTasks(int projectId, int id_task)
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

          var response = await _taskRepository.GetTasksAsync(projectId,id_task);
            if (response.Count == 0)
            {
                return StatusCode(400);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("createtoproject")]
        public async Task<IActionResult> CreateToProject(Tasks requast)
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

            Tasks task = new Tasks()
            {
                Title = requast.Title,
                Deadline = requast.Deadline,
                projectId = requast.projectId,
                Id_Task = 0,
                Priority = requast.Priority
            };

            await _taskRepository.CreateToProject(task);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("createtotask")]
        public async Task<IActionResult> CreateToTask(Tasks requast)
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

            Tasks task = new Tasks()
            {
                Title = requast.Title,
                Deadline = requast.Deadline,
                projectId = requast.projectId,
                Id_Task = requast.Id_Task,
                Priority = requast.Priority
            };

            await _taskRepository.CreateToTask(task);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("complite")]
        public async Task<IActionResult> Complite(int id)
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

            if (id != 0)
            {
               var pesponse = await _taskRepository.Complite(id);

                if (pesponse)
                {
                    return Ok(200);
                }
                else
                {
                    return BadRequest(400);
                }
            }
            else
            {
                return BadRequest(400);
            }
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("edittask")]
        public async Task<IActionResult> EditTask(Tasks requast)
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

            Tasks task = new Tasks()
            {
                Id = requast.Id,
                Title = requast.Title,
                Deadline = requast.Deadline,
               Priority = requast.Priority
            };

            await _taskRepository.EditTask(task);

            return Ok(200);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("delitetask")]
        public async Task<IActionResult> DeliteTask(int id)
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

            await _taskRepository.DeliteTask(id);

            return Ok(200);
        }

    }
}