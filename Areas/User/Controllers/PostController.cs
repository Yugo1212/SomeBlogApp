using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Areas.User.Controllers
{
    [Area("User")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Post post = new Post();

            if (id == null)
            {
                return View(post);
            }

            post = await _unitOfWork.Posts.GetFirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Post post)
        {
            if (ModelState.IsValid)
            {
                if(post.Id == 0)
                {
                   await _unitOfWork.Posts.AddAsync(post);
                }
                else
                {
                     _unitOfWork.Posts.Update(post);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index)); 
            }
            return View(post);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll(string userId)
        {
            var allPosts = await _unitOfWork.Posts.GetAllAsync(p => p.ApplicationUserId == userId);
            return Json(new { data = allPosts });
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var objFromDb = await _unitOfWork.Posts.GetByIdAsync(id);

            if (objFromDb == null)
                return NotFound();

            return View(objFromDb);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Posts.GetByIdAsync(id);

            if (objFromDb == null)
                return Json(new { success = "false", message = "Error while deleting" });

            await _unitOfWork.Posts.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = "true", message = "Delete succesful" });
        }
        #endregion
    }
}
