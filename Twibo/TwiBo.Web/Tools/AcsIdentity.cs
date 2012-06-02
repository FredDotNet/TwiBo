using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Claims;

namespace TwiBo.Web.Tools
{
    public class AcsIdentity
    {
        public string Name { get; set; }
        public string IdentityProvider { get; set; }

        public static AcsIdentity TryGet()
        {
            string idp = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
            string name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var claimsPrincipal = HttpContext.Current.User as IClaimsPrincipal;
            var claimsIdentity = (IClaimsIdentity)claimsPrincipal.Identity;
            if (claimsIdentity.Claims.Count(c => c.ClaimType == idp || c.ClaimType == name) == 2)
            {
                return new AcsIdentity()
                {
                    IdentityProvider = claimsIdentity.Claims.FirstOrDefault(o => o.ClaimType == idp).Value,
                    Name = claimsIdentity.Claims.FirstOrDefault(o => o.ClaimType == name).Value
                };
            }
            else
            {
                return null;
            }
        }
    }
}
