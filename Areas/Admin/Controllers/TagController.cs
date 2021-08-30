using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            Tag tag = new Tag();

            return View(tag);
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
               await _unitOfWork.Tags.AddAsync(tag);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        // POST: TagController/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb =await _unitOfWork.Tags.GetFirstOrDefaultAsync(o => o.Id == id);

            if (objFromDb == null)
                return Json(new { success = false, message = "Error while deleting" });
            await _unitOfWork.Tags.RemoveAsync(objFromDb.Id);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allTags = await _unitOfWork.Tags.GetAllAsync();
            return Json(new { data = allTags });
        }
        #endregion
    }
}
