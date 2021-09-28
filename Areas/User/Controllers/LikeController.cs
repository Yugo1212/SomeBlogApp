using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;

namespace TwitterCopyApp.Areas.User.Controllers
{
    public class LikeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> LikeUnlike(int postId)
        {
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            var likeInPostByCurrentUser = await _unitOfWork.Likes.GetFirstOrDefaultAsync(l => l.PostId == postId && l.ApplicationUserId == claim.Value);

            if(likeInPostByCurrentUser is not null)
            {
                if (likeInPostByCurrentUser.IsLiked == false)
                    likeInPostByCurrentUser.IsLiked = true;
                else
                    likeInPostByCurrentUser.IsLiked = false;

                return likeInPostByCurrentUser.IsLiked;
            }

            likeInPostByCurrentUser.PostId = postId;
            likeInPostByCurrentUser.ApplicationUserId = claim.Value;
            likeInPostByCurrentUser.IsLiked = true;
            await _unitOfWork.Likes.AddAsync(likeInPostByCurrentUser);
            _unitOfWork.Save();
            return likeInPostByCurrentUser.IsLiked;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<int> GetAllLikesInPost(int postId)
        {
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            var allLikes = await _unitOfWork.Likes.GetAllAsync(l => l.PostId == postId && l.ApplicationUserId == claim.Value && l.IsLiked == true);

            return allLikes.Count();
        }
    }
}
