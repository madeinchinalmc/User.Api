using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class CreateProjectCommand : IRequest<Project.Domain.AggregatesModel.Project>
    {
        public Project.Domain.AggregatesModel.Project project { get; set; }
    }
}
