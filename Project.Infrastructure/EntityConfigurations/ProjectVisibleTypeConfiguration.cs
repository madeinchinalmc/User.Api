using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectVisibleTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectVisibleRule>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectVisibleRule> projectConfiguration)
        {
            
            projectConfiguration.ToTable("projectvisiblerule").HasKey(t=>t.Id);
        }
    }
}
