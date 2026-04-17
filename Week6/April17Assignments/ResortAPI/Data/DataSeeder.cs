using Microsoft.AspNetCore.Identity;
using OnlineResortBooking.Models;

namespace OnlineResortBooking.Data
{
    public class DataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAdminAsync()
        {
            const string adminRole = "Admin";
            const string customerRole = "Customer";
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if (!await _roleManager.RoleExistsAsync(customerRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(customerRole));
            }

            var defaultAdmin = await _userManager.FindByEmailAsync("admin@resort.com");
            if (defaultAdmin == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@resort.com",
                    EmailConfirmed = true,
                    UserRole = adminRole,
                    MobileNumber = "0000000000"
                };

                var result = await _userManager.CreateAsync(admin, "Admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, adminRole);
                }
            }
        }
    }
}
