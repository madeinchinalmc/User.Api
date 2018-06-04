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
        public SmsAuthCodeGrantType()
        {

        }
        public string GrantType => throw new NotImplementedException();

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
