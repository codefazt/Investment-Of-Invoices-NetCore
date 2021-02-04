using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TuFactoringModels;

namespace TuFactoring.CustomProviders
{
    public class CustomClaimsStore: UserClaimsPrincipalFactory<User>
    {
        public CustomClaimsStore(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            return identity;
        }
    }
}
