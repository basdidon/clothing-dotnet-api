using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class RoleService(RoleManager<ApplicationRole> roleManager)
    {
        public async Task CreateRolesAsync(string[] rolesName)
        {
            foreach (var roleName in rolesName)
                await CreateRoleAsync(roleName);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                ApplicationRole role = new()
                {
                    Name = roleName
                };
                // Create role if it doesn't exist
                await roleManager.CreateAsync(role);
            }
        }
    }
}
