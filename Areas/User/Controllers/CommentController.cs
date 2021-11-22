using Microsoft.AspNetCore.Http;
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
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? commentId)
        {
            Comment comment = new Comment();

            if (commentId == null)
            {
                return View(comment);
            }

            comment = await _unitOfWork.Comments.GetFirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
                return NotFound();

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Comment comment, int postId)
        {
            var claimIdenitty = (ClaimsIdentity)User.Identity;
            var claim = claimIdenitty.FindFirst(ClaimTypes.NameIdentifier);

            try
            {
                if (comment.Id == 0)
                {
                    comment.CreationDate = DateTime.Now;
                    comment.User = await _unitOfWork.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == claim.Value);
                    comment.ApplicationUserId = claim.Value;
                    comment.Post = await _unitOfWork.Posts.GetFirstOrDefaultAsync(p => p.Id == postId);
                    comment.PostId = comment.Post.Id;
                    await _unitOfWork.Comments.AddAsync(comment);
                }
                else
                {
                    comment.CreationDate = DateTime.Now;
                    _unitOfWork.Comments.Update(comment);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index), "Home", new { area = "User" });
            }
            catch (Exception ex)
            {

                return View(ex);
            }
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAllCommentsInPost(int postId)
        {
            var allComments = await _unitOfWork.Comments.GetAllAsync(c => c.PostId == postId, includeProperties: "User");
            foreach(var comment in allComments)
            {
               comment.CreationDate = Convert.ToDateTime(comment.CreationDate.ToString("yyyy/MM/dd HH:mm"));
                
            }

            return Json(new { data = allComments }) ;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var objFromDb = await _unitOfWork.Comments.GetByIdAsync(id);


            if (objFromDb == null)
                return NotFound();

            return View(objFromDb);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Comments.GetByIdAsync(id);

            if (objFromDb == null)
                return Json(new { success = "false", message = "Error while deleting" });

            await _unitOfWork.Comments.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = "true", message = "Delete succesful" });
        }
        #endregion
    }
}
