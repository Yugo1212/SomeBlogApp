using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.User.Controllers
{

    [Area("User")]
    public class LikeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> LikeUnlike(int id, string entityName)
        {
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);
            var likedPost = await _unitOfWork.Posts.GetFirstOrDefaultAsync(p => p.Id == id);
            var likeInPostByCurrentUser = await _unitOfWork.Likes.GetFirstOrDefaultAsync(l => l.PostId == id && l.ApplicationUserId == claim.Value);
            var allLikes = await _unitOfWork.Likes.GetAllAsync(l => l.PostId == id);
            if (likeInPostByCurrentUser is not null)
            {
                if (likeInPostByCurrentUser.IsLiked == false)
                {
                    likeInPostByCurrentUser.IsLiked = true;
                    likedPost.Likes++;
                }
                else
                {
                    likeInPostByCurrentUser.IsLiked = false;
                    likedPost.Likes--;
                }

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
        #endregion
    }

}
