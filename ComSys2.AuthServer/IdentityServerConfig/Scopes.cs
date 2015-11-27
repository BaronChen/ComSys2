using System.Collections.Generic;
using IdentityManager;
using IdentityServer3.Core.Models;

namespace ComSys2.AuthServer.IdentityServerConfig
{
    public class Scopes
    {
		public static IEnumerable<Scope> Get()
		{
			return new[]
			{
                // standard OpenID Connect scopes
                StandardScopes.OpenId,
				StandardScopes.ProfileAlwaysInclude,
				StandardScopes.EmailAlwaysInclude,

                // API - access token will 
                // contain roles of user
                new Scope
				{
					Name = "api",
					DisplayName = "ComSys API",
					Type = ScopeType.Resource,

					Claims = new List<ScopeClaim>
					{
						new ScopeClaim(Constants.ClaimTypes.Role)
					}
				},
				new Scope
				{
					Name = "system",
					DisplayName = "ComSys System",
					Type = ScopeType.Resource,

					Claims = new List<ScopeClaim>
					{
						new ScopeClaim(Constants.ClaimTypes.Role)
					}
				}
			};
		}
	}
}