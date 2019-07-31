using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Model;

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserContext _userContext;
        private ILogger<UserController> _logger;
        public UserController(UserContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()

        {
            var user = await _userContext.Set<AppUser>()
                .AsNoTracking()
                .Include(u => u.userProperties)
                .FirstOrDefaultAsync(t => t.Id == 1);
            if (user == null)
            {
                _logger.LogError("登录用户为空");
                throw new UserOperationException("用户登录异常");
            }
            return Json(user);
        }
        /// <summary>
        /// 用户更新
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody]JsonPatchDocument<Model.AppUser> patch)
        {
            var user = await _userContext
                .Users
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            patch.ApplyTo(user);
            foreach (var property in user?.userProperties)
            {
                _userContext.Entry(property).State = EntityState.Detached;
            }
            var originProperties = await _userContext.UserProperties.AsNoTracking().Where(t => t.AppUserId == UserIdentity.UserId).ToListAsync();
            var allProperties = originProperties.Union(user.userProperties).Distinct();

            var removeProperties = originProperties.Except(user.userProperties);
            var newProperties = allProperties.Except(originProperties);
            foreach (var property in removeProperties)
            {
                _userContext.Entry(property).State = EntityState.Deleted;
            }
            foreach (var property in removeProperties)
            {
                _userContext.Entry(property).State = EntityState.Added;
            }

            _userContext.Users.Update(user);
            _userContext.SaveChanges();
            return Json(user);
        }

        [Route("check_or_create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);

            if (user == null)
            {
                user = new AppUser { Phone = phone };
                _userContext.Users.Add(new AppUser { Phone = phone });
                await _userContext.SaveChangesAsync();
            }
            return Ok(new {
                user.Id,
                user.Name,
                user.Company,
                user.Title,
                user.Avatar
            });
        }
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            return Json(await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync());
        }
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(string phone)
        {
            return Json(await _userContext.Users.Include(u => u.userProperties).SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId));
        }
        [HttpPost]
        [Route("tags" +
            "")]
        public async Task<IActionResult> UpdateUserTags([FromBody] List<string>tags)
        {
            var originTas =await _userContext.UserTags.Where(t => t.UserId == UserIdentity.UserId).ToListAsync();
            //originTas.Select(t=>t.Tag).Except(tags);
            var newTags = tags.Except(originTas.Select(t => t.Tag));
            await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new UserTag { CreateTime = DateTime.Now, Tag = t, UserId = UserIdentity.UserId }));
            await _userContext.SaveChangesAsync();
            return Ok();
        }
    }
}
