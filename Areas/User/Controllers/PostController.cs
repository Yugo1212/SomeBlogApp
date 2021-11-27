using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Areas.User.Controllers
{
    [Area("User")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public PostController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Post post = new Post();
            if(post.Comments is null)
                post.Comments = new List<Comment>();

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
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            try
            {
                if(post.Id == 0)
                {
                    post.CreationDate = DateTime.Now;
                    post.ApplicationUserId = claim.Value;
                    await _unitOfWork.Posts.AddAsync(post);
                }
                else
                {
                    post.CreationDate = DateTime.Now;
                    post.ApplicationUserId = claim.Value;
                    _unitOfWork.Posts.Update(post);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index), "Home", new { area = "User"}); 
            }
            catch(Exception ex)
            {
                
                return View(ex);
            }
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll(string userId)
        {
            var allPosts = await _unitOfWork.Posts.GetAllAsync(p => p.ApplicationUserId == userId);
            return View(allPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var objFromDb = await _unitOfWork.Posts.GetByIdAsync(id);

            if (objFromDb == null)
                return NotFound();

            return View(objFromDb);
        }

        public async Task<IActionResult> LikeUnlike(int id)
        {
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            var likedPost = await _unitOfWork.Posts.GetFirstOrDefaultAsync(p => p.Id == id);
            var likeInPostByCurrentUser = await _unitOfWork.Likes.GetFirstOrDefaultAsync(l => l.PostId == id && l.ApplicationUserId == claim.Value);

            if (likeInPostByCurrentUser is not null)
            {
                likeInPostByCurrentUser.IsLiked = !likeInPostByCurrentUser.IsLiked;

                if (likeInPostByCurrentUser.IsLiked == true)
                    likedPost.Likes++;
                else
                    likedPost.Likes--;

                _unitOfWork.Posts.Update(likedPost);
                _unitOfWork.Save();
                return Json(new { success = likeInPostByCurrentUser.IsLiked });
            }
            likeInPostByCurrentUser = new Like()
            {
                PostId = id,
                ApplicationUserId = claim.Value,
                IsLiked = true,
            };
            likedPost.Likes++;

            await _unitOfWork.Likes.AddAsync(likeInPostByCurrentUser);
            _unitOfWork.Save();

            return Json(new { success = likeInPostByCurrentUser.IsLiked });
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
