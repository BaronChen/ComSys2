using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComSys2.AuthServer.IdentityServerConfig;
using ComSys2.AuthServer.IdentityServerConfig.ManageServices;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using IdentityServer3.Core.Configuration;
using Owin;

namespace ComSys2.AuthServer
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.Map("/admin", adminApp =>
			{
				var factory = new IdentityManagerServiceFactory();
				factory.ConfigureSimpleIdentityManagerService();

				adminApp.UseIdentityManager(new IdentityManagerOptions()
				{
					Factory = factory
				});
			});

			app.Map("/core", core =>
			{
				var idSvrFactory = Factory.Configure();
				idSvrFactory.ConfigureUserService();

				var options = new IdentityServerOptions
				{
					SiteName = "IdentityServer3 - UserService-AspNetIdentity",
					SigningCertificate = Certificate.Get(),
					Factory = idSvrFactory,
					AuthenticationOptions = new AuthenticationOptions
					{
						IdentityProviders = ConfigureAdditionalIdentityProviders,
					}
				};

				core.UseIdentityServer(options);
			});

			ConfigureAuth(app);
		}

		public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
		{
			
		}
	}
}