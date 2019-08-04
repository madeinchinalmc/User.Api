using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {

        private IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectContributor.ProjectId);
            if (project == null)
                throw new Domain.Exceptions.ProjectDomainException("project not found");
            project.AddContributor(request.ProjectContributor);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return new Unit();
        }
    }
}
