using AceRental.IdentityServer;
using AceRental.IdentityServer.Models;
using AceRental.IdentityServer.Services;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;

namespace AceRental.IdentityServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // 1. Récupérer l'utilisateur actuel
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user != null)
            {
                // 2. Récupérer ses rôles depuis ta base de données dédiée
                var roles = await _userManager.GetRolesAsync(user);

                // 3. Construire l'objet que tu veux imbriquer
                var claimsObject = new
                {
                    roles = roles,
                    email = user.Email,
                    username = user.UserName
                };

                // 4. Sérialiser l'objet en texte JSON
                var jsonString = JsonSerializer.Serialize(claimsObject);

                // 5. Créer le Claim "claims" en spécifiant le type "Json"
                var customClaim = new Claim(
                    "claims",
                    jsonString,
                    IdentityServerConstants.ClaimValueTypes.Json // Indispensable pour éviter d'avoir une simple string
                );

                // 6. L'ajouter au jeton
                context.IssuedClaims.Add(customClaim);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
