using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.Data;
using TwitterCopyApp.DataAccess.Repository.IRepository;

namespace TwitterCopyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUsers =  _db.ApplicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach(var user in allUsers)
            {
               var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
            }

            return Json(new { data = allUsers });
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string Id)
        {
            var userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == Id);

            if (userFromDb is null)
                return Json(new { success = false, message = "Error while locking/unlocikng" });

            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
                userFromDb.LockoutEnd = DateTime.Now;
            else
                userFromDb.LockoutEnd = DateTime.Now.AddDays(31);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Operation successful" });
        }
        #endregion
    }
}
