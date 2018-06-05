using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Service;

namespace User.API.Authentication
{
    public class SmsAuthCodeGrantType : IExtensionGrantValidator
    {
        private IUserService _userService;
        private IAuthCodeService _iAuthCodeService;
        public SmsAuthCodeGrantType(IUserService userService, IAuthCodeService iAuthCodeService)
        {
            _userService = userService;
            _iAuthCodeService = iAuthCodeService;
        }
        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            if(string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //if(!_iAuthCodeService.val)
        }
    }
}
