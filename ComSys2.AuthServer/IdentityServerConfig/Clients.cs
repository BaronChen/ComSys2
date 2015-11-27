using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;

namespace WebHost.IdSvr
{
    public class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
				   ClientName = "Test Client",
					ClientId = "test",
					ClientSecrets = new List<Secret>
					{
						new Secret("testSecret".Sha256())
					},

                    // server to server communication
                    Flow = Flows.ResourceOwner,
					AccessTokenType = AccessTokenType.Reference,
                    // only allowed to access api1
                    AllowedScopes = new List<string>
					{
						"api",
						"system"
					}
				}
            };
        }
    }
}