using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Infrastructure
{
    public static class Seed
    {
        public static void SeedRoles(IEnumerable<string> roles, RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in roles)
            {
                var role = new IdentityRole { Name = roleName, NormalizedName = roleName, ConcurrencyStamp = Guid.NewGuid().ToString() };
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    roleManager.CreateAsync(role).Wait();
                }
            }

        }
    }
}
