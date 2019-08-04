using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectContributor:Entity
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
