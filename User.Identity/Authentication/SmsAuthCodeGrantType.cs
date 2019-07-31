using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.Identity.Service;

namespace User.Identity.Authentication
{
    //自定义验证
    public class SmsAuthCodeGrantType : IExtensionGrantValidator
    {
        private IUserService _userService;
        private IAuthCodeService _authCodeService;
        public SmsAuthCodeGrantType(IUserService userService, IAuthCodeService authCodeService)
        {
            _userService = userService;
            _authCodeService = authCodeService;
        }
        public string GrantType => "sms_auth_code";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);


            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //检查验证码
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //完成用户注册
            var userinfo = await _userService.CheckOrCreate(phone);
            if (userinfo== null)
            {
                context.Result = errorValidationResult;
                return;
            }
            var claims = new Claim[]
            {
                new Claim("name",userinfo.Name??string.Empty),
                new Claim("company",userinfo.Company??string.Empty),
                new Claim("title",userinfo.Tiltle??string.Empty),
                new Claim("avatar",userinfo.Avatar??string.Empty),
            }; 
            context.Result = new GrantValidationResult(userinfo.Id.ToString(), 
                GrantType,
                claims);
        }
    }
}
