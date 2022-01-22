using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Verbum.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("VerbumWebAPI", "WebAPI")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource> {
                new ApiResource("VerbumWebAPI", "WebAPI", new [] {JwtClaimTypes.Name}){
                    Scopes = { "VerbumWebAPI" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> {
                new Client
                {
                    ClientId = "verbum-web-api",
                    ClientName = "Verbum Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = {
                        //"http://localhost:8080/oidc-callback"
                        "http://localhost:8080/auth/signinwin/main"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:8080"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:8080/"
                    },
                    AllowedScopes = { 
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       "VerbumWebAPI"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
