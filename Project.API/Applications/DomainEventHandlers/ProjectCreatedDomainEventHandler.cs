using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.Domain.Events;
using Project.API.Applications.IntegrationEvents;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntegrationEvent { ProjectId = notification.Project.Id,UserId = notification.Project.UserId, CreatedTime = DateTime.Now};
            _capPublisher.Publish("findbook.projectapi.projectcreated ", @event);
            return Task.CompletedTask;
        }
    }
}
