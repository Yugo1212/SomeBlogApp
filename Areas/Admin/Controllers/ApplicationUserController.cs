using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.Data;
using TwitterCopyApp.Models.ViewModels;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using System.Security.Claims;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public UserPostViewModel userPostViewModel;

        public ApplicationUserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetUserDashboard(string id)
        {
            UserPostViewModel userVM = new UserPostViewModel()
            {
                User = await _unitOfWork.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == id),
                UserPosts = await _unitOfWork.Posts.GetAllAsync(p => p.ApplicationUserId == id, includeProperties: "Comments"),
                Likes = await _unitOfWork.Likes.GetAllAsync()
            };

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> FollowUnfollow(string id)
        {

            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            var userToBeAdded = await _unitOfWork.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == claim.Value);

            var user = await _unitOfWork.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == id);

            if (user.FollowedUsers is null)
                user.FollowedUsers = new List<ApplicationUser>();

            if (!user.FollowedUsers.Contains(userToBeAdded))
            {
                user.FollowedUsers.Add(userToBeAdded);

                _unitOfWork.Save();

                return Json(new { success = true });
            }

            user.FollowedUsers.Remove(userToBeAdded);
            _unitOfWork.Save();

            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult GetAll()
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
        public IActionResult LockUnlock([FromBody] string Id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == Id);
            if (objFromDb == null)
                return Json(new { success = false, message = "Error while locking/unlocking" });

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
                objFromDb.LockoutEnd = DateTime.Now;
            else
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            _db.SaveChanges();
            return Json(new { success = true, message = "Operation succesful" });
        }
        #endregion
    }
}
