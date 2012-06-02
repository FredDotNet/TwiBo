using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Claims;
using TwiBo.Web.Tools;
using TwiBo.Components;
using TwiBo.Components.Model;

namespace TwiBo.Web
{
    public class ClaimsManager : ClaimsAuthenticationManager
    {
        public override IClaimsPrincipal Authenticate(string resourceName, IClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal.Identity.IsAuthenticated)
            {

                var identity = AcsIdentity.TryGet(incomingPrincipal.Identity);
                var client = new TableStorageClient<User>(TwiBo.Components.Model.User.TableName);
                client.Upsert(new User()
                {
                    PartitionKey = TwiBo.Components.Model.User.GetPartitionKey(),
                    RowKey = TwiBo.Components.Model.User.GetRowKey(identity.IdentityProvider, identity.Name)
                });
                client.SaveChanges();
            }

            return base.Authenticate(resourceName, incomingPrincipal);
        }
    }
}