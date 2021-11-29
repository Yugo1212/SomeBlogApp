using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterCopyApp.Data;
using TwitterCopyApp.Models;
using TwitterCopyApp.Utility;

namespace TwitterCopyApp.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initalize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (_db.Roles.Any(r => r.Name == Roles.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_User)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Moderator)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin",
                Email = "adam_kaszynski@tlen.pl",
                EmailConfirmed = true,
                FollowedUsers = new List<ApplicationUser>(),
                FirstName = "Adam",
                LastName = "Kaszyński"

            }, "123Ab#").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "adam_kaszynski@tlen.pl").FirstOrDefault();

            _userManager.AddToRoleAsync(user, Roles.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
