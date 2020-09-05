using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Contracts;
using WebUI.Models.Dto;
using WebUI.Models.Dtos;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediaFileManager _manager;

        public HomeController(IMediaFileManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListAll()
        {
            var records = await _manager.GetAll();
            return View(records);
        }

        public IActionResult Upload()
        {
            return View(new FileDto { });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileDto file)
        {
            var result = await _manager.Upload(file);
            TempData["FlasResult"] = result;
            TempData["FlashMessage"] = result ?
                "You video is saved successfully!" :
                "Something went wrong. Please check to submit correct video file (limit 50 MB).";

            return Redirect("ListAll");
        }


        public async Task<IActionResult> Display(long id)
        {
            var record = await _manager.Get(id);
            return View(record);
        }

        public IActionResult Search()
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
