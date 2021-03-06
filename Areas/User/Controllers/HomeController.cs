using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwitterCopyApp.DataAccess.Repository.IRepository;
using TwitterCopyApp.Models;
using TwitterCopyApp.Models.ViewModels;

namespace TwitterCopyApp.Controllers.Customer
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PostViewModel PostVM;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            PostVM = new PostViewModel()
            {
                Posts = await _unitOfWork.Posts.GetAllAsync(includeProperties: "User,Comments"),
                Likes = await _unitOfWork.Likes.GetAllAsync(includeProperties: "User")
            };
            return View(PostVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
