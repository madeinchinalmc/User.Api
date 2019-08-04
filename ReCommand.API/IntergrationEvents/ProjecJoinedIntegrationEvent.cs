using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;

namespace ReCommend.API.IntegrationEvents
{
    public class ProjecJoinedIntegrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public ProjectContributor Contributor { get; set; }
    }
}
