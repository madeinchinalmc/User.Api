using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.API.Applications.Queries;
using Project.API.Applications.Service;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IRecommendService _recommendService;
        private readonly IProjectQueries _projectQueries;
        private readonly ICapPublisher _capPublisher;

        public ProjectController(IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries, ICapPublisher capPublisher)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
            _capPublisher = capPublisher;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProject([FromBody] Domain.AggregatesModel.Project project)
        {
            var command = new CreateProjectCommand()
            {
                project = project
            };
            var result =await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        [Route("views/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if(await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest();
            }
            var command = new ViewProjectCommand()
            {
                ProjectId = projectId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        [Route("joins/{projectId}")]
        public async Task<IActionResult> JoinProject([FromBody] ProjectContributor projectContributor)
        {
            if (await _recommendService.IsProjectInRecommend(projectContributor.ProjectId, UserIdentity.UserId))
            {
                return BadRequest();
            }
            var command = new JoinProjectCommand()
            {
                ProjectContributor = projectContributor
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectQueries.GetProjectsByUserId(UserIdentity.UserId);
            return Ok(projects);
        }
        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetMyProjectDetails(int projectId)
        {
            var project = await _projectQueries.GetProejctDetail( projectId);
            if(project.UserId == UserIdentity.UserId)
            {
                return Ok(project);
            }
            else
            {
                return BadRequest("无权查看项目");
            }
        }
        [Route("recommends/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendsProjectDetails(int projectId)
        {
            if (await _recommendService.IsProjectInRecommend(projectId,UserIdentity.UserId))
            {
                var project = await _projectQueries.GetProejctDetail(projectId);
                return Ok(project);
            }
            else
            {
                return BadRequest("无权查看项目");
            }
        }
    }
}
