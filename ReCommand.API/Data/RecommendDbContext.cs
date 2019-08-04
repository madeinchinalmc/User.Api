using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReCommend.API.Modles;

namespace ReCommend.API.Data
{
    public class RecommendDbContext:DbContext
    {
        public RecommendDbContext(DbContextOptions<RecommendDbContext> dbContextOptions):base(dbContextOptions)
        {

        }
        public DbSet<ProjectRecommend> projectRecommends { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectRecommend>().ToTable("ProjectRecommends")
                .HasKey(t=>t.Id);

            modelBuilder.Entity<ProjectRecommendUser>().ToTable("ProjectRecommendUser")
                .HasKey(t => new { t.ProjectRecommendId, t.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
