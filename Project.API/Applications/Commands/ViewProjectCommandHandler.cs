using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.API.Applications.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        private IProjectRepository _projectRepository;
        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);
            if (project == null)
                throw new Domain.Exceptions.ProjectDomainException("project not found");
            if(project.UserId == request.UserId)
                throw new Domain.Exceptions.ProjectDomainException("you cannot view own project");
            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return new Unit();
        }
    }
}
