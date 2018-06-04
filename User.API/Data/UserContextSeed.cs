using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace User.API.Data
{
    public class UserContextSeed
    {
        private ILogger<UserContextSeed> _logger;
        public UserContextSeed(ILogger<UserContextSeed> logger)
        {
            _logger = logger;
        }
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,int?retry = 0)
        {
            var retryForAvaiability = retry.Value;
            try
            {
                using(var scope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var context = (UserContext)scope.ServiceProvider.GetService(typeof(UserContext));
                    var logger = (ILogger<UserContextSeed>)scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));
                    logger.LogDebug("Begin UserContextSeed SeedAsync");
                    //获取到数据库的migrate
                    context.Database.Migrate();
                    if (!context.Users.Any())
                    {
                        context.Users.Add(new Model.AppUser { Name = "lmc" });
                        context.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                if(retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    var logger = loggerFactory.CreateLogger(typeof(UserContextSeed));
                    await SeedAsync(applicationBuilder, loggerFactory, retryForAvaiability);
                }
            }
        }
    }
}
