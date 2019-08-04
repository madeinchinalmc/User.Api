using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Service
{
    public class TestRecommendService : IRecommendService
    {
        public async Task<bool> IsProjectInRecommend(int ProjectId, int userId)
        {
            return await Task.FromResult(true);
        }
    }
}
