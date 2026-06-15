using System.Security.Claims;
using Duende.IdentityModel;
using AceRental.IdentityServer.Data;
using AceRental.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net.Http.Headers;

namespace AceRental.IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            CreateRoles(roleMgr);

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            CreateUsers(userMgr);
        }
    }

    private static void CreateRoles(RoleManager<IdentityRole> roleMgr)
    {
        var rolesToCreate = new[] { "Admin", "Commercial", "Stock", "client" };

        foreach (var roleName in rolesToCreate)
        {
            if (!roleMgr.RoleExistsAsync(roleName).Result)
            {
                var role = new IdentityRole(roleName);
                var result = roleMgr.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug($"role {roleName} created");
            }
            else
            {
                Log.Debug($"role {roleName} already exists");
            }
        }
    }

    private static void CreateUsers(UserManager<ApplicationUser> userMgr)
    {
        // var alice = userMgr.FindByNameAsync("alice").Result;
        // if (alice == null)
        // {
        //     alice = new ApplicationUser
        //     {
        //         UserName = "alice",
        //         Email = "AliceSmith@example.com",
        //         EmailConfirmed = true,
        //     };
        //     var result = userMgr.CreateAsync(alice, "Pass123$").Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }

        //     result = userMgr.AddClaimsAsync(alice, new Claim[]{
        //                     new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //                     new Claim(JwtClaimTypes.GivenName, "Alice"),
        //                     new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                     new Claim(JwtClaimTypes.WebSite, "http://alice.example.com"),
        //                 }).Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }

        //     result = userMgr.AddToRoleAsync(alice, "client").Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }
        //     Log.Debug("alice created");
        // }
        // else
        // {
        //     Log.Debug("alice already exists");
        // }

        // var bob = userMgr.FindByNameAsync("bob").Result;
        // if (bob == null)
        // {
        //     bob = new ApplicationUser
        //     {
        //         UserName = "bob",
        //         Email = "BobSmith@example.com",
        //         EmailConfirmed = true
        //     };
        //     var result = userMgr.CreateAsync(bob, "Pass123$").Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }

        //     result = userMgr.AddClaimsAsync(bob, new Claim[]{
        //                     new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //                     new Claim(JwtClaimTypes.GivenName, "Bob"),
        //                     new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                     new Claim(JwtClaimTypes.WebSite, "http://bob.example.com"),
        //                     new Claim("location", "somewhere")
        //                 }).Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }

        //     result = userMgr.AddToRoleAsync(bob, "client").Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new Exception(result.Errors.First().Description);
        //     }
        //     Log.Debug("bob created");
        // }
        // else
        // {
        //     Log.Debug("bob already exists");
        // }

        var admin = userMgr.FindByNameAsync("admin").Result;
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "hatembeddaira@gmail.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(admin, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Daira Hatem"),
                            new Claim(JwtClaimTypes.GivenName, "Hatem"),
                            new Claim(JwtClaimTypes.FamilyName, "Daira"),
                            new Claim("location", "91700")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddToRoleAsync(admin, "Admin").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("admin created");
        }
        else
        {
            Log.Debug("admin already exists");
        }

    }

}
