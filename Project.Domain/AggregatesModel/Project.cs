using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project.Domain.Events;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class Project:Entity, IAggregateRoot
    {
        public Project()
        {
            this.Viewers = new List<ProjectViewer>();
            this.Contributors = new List<ProjectContributor>();
            this.Properties = new List<ProjectProperty>();
            this.AddDomainEvent(new ProjectCreatedEvent() {Project = this });
        }
        public int UserId { get; set; }
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Acator { get; set; }
        public virtual IList<ProjectViewer> Viewers { get; set; }
        public virtual IList<ProjectContributor> Contributors { get; set; }
        public virtual IList<ProjectProperty> Properties { get; set; }
        public void AddViewer(int userId,string userName,string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreatedTime= DateTime.Now,
            };
            if(!Viewers.Any(v =>v.UserId == userId))
            {
                Viewers.Add(viewer);
                AddDomainEvent(new ProjectViewedEvent() {
                    Company = this.Company,
                    Introduction = this.Introduction,
                    ProjectViewer = viewer });
            }
        }
        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == UserId))
            {
                Contributors.Add(contributor);
                AddDomainEvent(new ProejctJoinedEvent() {
                    Company = this.Company,
                    Introduction = this.Introduction,
                    Acator = this.Acator,
                    ProjectContributor = contributor
                });
            }

        }
    }
}
