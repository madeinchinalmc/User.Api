using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommend.API.IntegrationEvents;
using ReCommend.API.Data;
using ReCommend.API.Modles;

namespace ReCommend.API.IntegrationEventHandlers
{
    public class ProjectCreatedIntegrationEventHandler:ICapSubscribe
    {
        private readonly RecommendDbContext _context;
        public ProjectCreatedIntegrationEventHandler(RecommendDbContext context)
        {
            _context = context;
        }
        public Task CreateRecommendFromProject(ProjectCreatedIntegrationEvent @event)
        {
            var recommend = new ProjectRecommend()
            {
                UserId = @event.UserId,
                ProjectId = @event.ProjectId
            };
            return Task.CompletedTask;
        }
    }
}
