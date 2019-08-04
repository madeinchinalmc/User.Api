using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Events
{
    public class ProejctJoinedEvent : INotification
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Acator { get; set; }
        public Project.Domain.AggregatesModel.ProjectContributor ProjectContributor { get; set; }
    }
}
