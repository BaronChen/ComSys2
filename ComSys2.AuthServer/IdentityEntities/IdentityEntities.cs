using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IdSvr3 = IdentityServer3.Core;

namespace ComSys2.AuthServer.IdentityEntities
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here
			return userIdentity;
		}

	}

	public class Role : IdentityRole { }

	public class AuthDbContext : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
	{
		public AuthDbContext()
			: base("AuthDb")
		{
		}

		public AuthDbContext(string connString)
			: base(connString)
		{
		}

		public static AuthDbContext Create()
		{
			return new AuthDbContext();
		}
	}

	public class UserStore : UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
	{
		public UserStore(AuthDbContext ctx)
			: base(ctx)
		{
		}
	}

	public class UserManager : UserManager<User, string>
	{
		public UserManager(UserStore store)
			: base(store)
		{
			this.ClaimsIdentityFactory = new ClaimsFactory();
		}

		public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
		{
			var manager = new UserManager(new UserStore(context.Get<AuthDbContext>()));
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<User>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false,
			};
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ComSys Identity"));
			}
			return manager;
		}
	}

	public class ClaimsFactory : ClaimsIdentityFactory<User, string>
	{
		public ClaimsFactory()
		{
			this.UserIdClaimType = IdSvr3.Constants.ClaimTypes.Subject;
			this.UserNameClaimType = IdSvr3.Constants.ClaimTypes.PreferredUserName;
			this.RoleClaimType = IdSvr3.Constants.ClaimTypes.Role;
		}

		public override async System.Threading.Tasks.Task<System.Security.Claims.ClaimsIdentity> CreateAsync(UserManager<User, string> manager, User user, string authenticationType)
		{
			var ci = await base.CreateAsync(manager, user, authenticationType);
			if (!string.IsNullOrWhiteSpace(user.FirstName))
			{
				ci.AddClaim(new Claim("given_name", user.FirstName));
			}
			if (!string.IsNullOrWhiteSpace(user.LastName))
			{
				ci.AddClaim(new Claim("family_name", user.LastName));
			}
			return ci;
		}
	}

	public class RoleStore : RoleStore<Role>
	{
		public RoleStore(AuthDbContext ctx)
			: base(ctx)
		{
		}
	}

	public class RoleManager : RoleManager<Role>
	{
		public RoleManager(RoleStore store)
			: base(store)
		{
		}
	}


}