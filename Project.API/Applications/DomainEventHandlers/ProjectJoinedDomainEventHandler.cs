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
    public class ProjectJoinedDomainEventHandler : INotificationHandler<ProejctJoinedEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public ProjectJoinedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public Task Handle(ProejctJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjecJoinedIntegrationEvent { Company = notification.Company, Introduction = notification.Introduction,Contributor = notification.ProjectContributor};
            _capPublisher.Publish("findbook.projectapi.projectjoin ", @event);
            return Task.CompletedTask;
        }
    }
}
