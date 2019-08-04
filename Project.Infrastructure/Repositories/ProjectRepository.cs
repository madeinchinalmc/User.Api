using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }
        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }
        public Domain.AggregatesModel.Project Add(Domain.AggregatesModel.Project project)
        {
            if (project.IsTransient())
            {

                return  _context.Add(project).Entity;
            }
            return project;
        }

        public async Task<Domain.AggregatesModel.Project> GetAsync(int id)
        {
            return await _context
                .Projects
                .Include(p => p.Viewers)
                .Include(p => p.Contributors)
                .FirstAsync(t=>t.Id == id);
        }

        public Domain.AggregatesModel.Project Update(Domain.AggregatesModel.Project project)
        {
           var projectResult = _context.Update(project)
                .Entity;
            return projectResult;
        }
    }
}
