using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Seed
{
    public static class IdeentitySeed
    {
        public static async Task SeedAsync(IServiceProvider sp) 
        {
            using var scope = sp.CreateScope();
            var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roles = new[] {"Admin" , "Customer" };
            foreach (var r in roles)
            {
                if(! await roleManger.RoleExistsAsync(r))
                    await roleManger.CreateAsync(new IdentityRole(r));
            }
            // Admin User
            var adminEmail = "admin@InventoryApp.com";
            var admin =await userManger.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser {UserName = adminEmail, Email = adminEmail, Fname = "System", Lname = "Admin"};
                await userManger.CreateAsync(admin, "Admin@123");
                await userManger.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
