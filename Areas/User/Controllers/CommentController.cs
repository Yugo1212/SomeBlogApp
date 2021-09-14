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

        public async Task<IActionResult> Upsert(int? id)
        {
            Comment comment = new Comment();

            if (id == null)
            {
                return View(comment);
            }

            comment = await _unitOfWork.Comments.GetFirstOrDefaultAsync(p => p.Id == id);
            if (comment == null)
                return NotFound();

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if(comment.Id == 0)
                {
                   await _unitOfWork.Comments.AddAsync(comment);
                }
                else
                {
                     _unitOfWork.Comments.Update(comment);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index)); 
            }
            return View(comment);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAllCommentsInPost(int postId)
        {
            var allComments = await _unitOfWork.Comments.GetAllAsync(c => c.PostId == postId);
            return Json(new { data = allComments });
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
