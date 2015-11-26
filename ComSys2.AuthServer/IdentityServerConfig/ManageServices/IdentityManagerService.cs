using ComSys2.AuthServer.IdentityEntities;
using IdentityManager;
using IdentityManager.AspNetIdentity;
using IdentityManager.Configuration;

namespace ComSys2.AuthServer.IdentityServerConfig.ManageServices
{
	public class IdentityManagerService : AspNetIdentityManagerService<User, string, Role, string>
	{
		public IdentityManagerService(UserManager userManager, RoleManager roleManager) : base(userManager, roleManager)
		{
			
		}
	}

	public static class IdentityManagerServiceFactoryExtension
	{
		public static void ConfigureSimpleIdentityManagerService(this IdentityManagerServiceFactory factory)
		{

			factory.Register(new Registration<AuthDbContext>(resolver => new AuthDbContext()));
			factory.Register(new Registration<UserStore>());
			factory.Register(new Registration<RoleStore>());
			factory.Register(new Registration<UserManager>());
			factory.Register(new Registration<RoleManager>());
			factory.IdentityManagerService = new Registration<IIdentityManagerService, IdentityManagerService>();
		}
	}

}