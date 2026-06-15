using Duende.IdentityServer.Models;

namespace AceRental.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("api", "Accès à l'API principale", new[] 
            { 
                "role",     // Indispensable pour ton [Authorize(Roles = "Admin")]
                "name",     // Optionnel : Pour afficher le nom de l'utilisateur
                "email"     // Optionnel : Pour l'email
            })
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
            new Client
            {
                ClientId = "ace-rental-angular",
                ClientName = "Mon Application Angular 21",
                AllowedGrantTypes = GrantTypes.Code, // Requis pour PKCE
                RequirePkce = true,
                RequireClientSecret = false, // Une SPA ne peut pas cacher de secret

                RedirectUris = { 
                    "http://localhost:4200/index.html", 
                    "https://oauth.pstmn.io/v1/callback" 
                }, // URL de ton Angular local
                PostLogoutRedirectUris = { "http://localhost:4200/index.html" },
                AllowedCorsOrigins = { "http://localhost:4200", "https://oauth.pstmn.io"}, // Éviter les erreurs CORS

                AllowedScopes = { "openid", "profile", "email", "api" }
            }
        };
}
