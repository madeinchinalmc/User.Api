using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.Project>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.Project> projectConfiguration)
        {
            
            projectConfiguration.ToTable("projects").HasKey(t=>t.Id);
        }
    }
}
