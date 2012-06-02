using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.IdentityModel.Claims;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using TwiBo.Web.Tools;
using TwiBo.Components.Model;
using TwiBo.Components;
using Microsoft.IdentityModel.Web;

namespace TwiBo.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) => { configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)); });

            FederatedAuthentication.ServiceConfigurationCreated += new EventHandler<Microsoft.IdentityModel.Web.Configuration.ServiceConfigurationCreatedEventArgs>(OnConfigCreated);
        }

        void OnConfigCreated(object sender, Microsoft.IdentityModel.Web.Configuration.ServiceConfigurationCreatedEventArgs e)
        {
            FederatedAuthentication.WSFederationAuthenticationModule.SecurityTokenValidated += new EventHandler<SecurityTokenValidatedEventArgs>(OnLogin);    
        }

        void OnLogin(object sender, SecurityTokenValidatedEventArgs e)
        {            
            if (Request.IsAuthenticated)
            {
                var identity = AcsIdentity.TryGet();
                var client = new TableStorageClient<User>(TwiBo.Components.Model.User.TableName);
                client.Upsert(new User()
                {
                    PartitionKey = TwiBo.Components.Model.User.GetPartitionKey(),
                    RowKey = TwiBo.Components.Model.User.GetRowKey(identity.IdentityProvider, identity.Name)
                });
                client.SaveChanges();
            }
        }
    }
}