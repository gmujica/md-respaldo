using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MesaDinero.Admin.Infrastructure
{
    public static class IdentityExtensions
    {
        public static Claim GetClaimByType(this IPrincipal principal, string claimType)
        {
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            var claim = claimsIdentity != null ? null : claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
            return claim;
        }

        public static string GetClaimValueByType(this IPrincipal principal, string claimType)
        {

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.PostalCode)
                .Select(c => c.Value).SingleOrDefault();
            

         


            var claim = GetClaimByType(principal, claimType);
            return id;
        }
    }
}