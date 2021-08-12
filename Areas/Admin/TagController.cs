using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Areas.Admin
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            Tag tag = new Tag();

            return View(tag);
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Tags.AddAsync(tag);
            }
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        // POST: TagController/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Tags.GetFirstOrDefaultAsync(o => o.Id == id);

            if (objFromDb == null)
                return Json(new { success = false, message = "Error while deleting" });
            _unitOfWork.Tags.RemoveAsync(objFromDb.Id);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted" });
        }
        #endregion
    }
}
