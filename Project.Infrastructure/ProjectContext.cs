using Microsoft.EntityFrameworkCore;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Infrastructure.EntityConfigurations;

namespace Project.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {
        public DbSet<Domain.AggregatesModel.Project> Projects { get; set; }
        private readonly IMediator _mediator;
        public ProjectContext(DbContextOptions<ProjectContext> options, IMediator mediator) :base(options)
        {
            _mediator = mediator;
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectContributorTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
