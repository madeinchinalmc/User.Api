using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectPropertyConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectProperty> projectConfiguration)
        {
            projectConfiguration.ToTable("projectproperty")
                .Property(p => p.Key).HasMaxLength(100);
            projectConfiguration
                .HasKey(t=>new { t.ProjectId,t.Key,t.Value });
            projectConfiguration
                .Property(p => p.Value).HasMaxLength(100);
        }
    }
}
