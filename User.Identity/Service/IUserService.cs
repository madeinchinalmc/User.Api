using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 检查手机号是否注册，未注册就注册
        /// </summary>
        /// <param name="phone"></param>
        Task<UserInfo> CheckOrCreate(string phone);
    }
}
