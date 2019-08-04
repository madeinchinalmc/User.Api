using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectContributorTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectContributor> projectConfiguration)
        {
            
            projectConfiguration.ToTable("projectscontributor").HasKey(t=>t.Id);
        }
    }
}
