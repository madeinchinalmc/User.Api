using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectViewDomainEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public ProjectViewDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedIntegrationEvent { Company = notification.Company,Introduction = notification.Introduction,Viewer = notification.ProjectViewer};
            _capPublisher.Publish("findbook.projectapi.projectview ", @event);
            return Task.CompletedTask;
        }
    }
}
