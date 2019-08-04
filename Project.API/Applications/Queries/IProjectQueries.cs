using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectsByUserId(int userId);
        Task<dynamic> GetProejctDetail(int projectId);
    }
}
