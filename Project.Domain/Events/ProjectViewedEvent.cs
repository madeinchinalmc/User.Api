using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Project.Domain.Events
{
    public class ProjectViewedEvent : INotification
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public Project.Domain.AggregatesModel.ProjectViewer ProjectViewer { get; set; }
    }
}
