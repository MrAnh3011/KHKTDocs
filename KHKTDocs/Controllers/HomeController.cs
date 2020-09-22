using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KHKTDocs.Models;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Entities;

namespace KHKTDocs.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDocumentService _documentService;
        private readonly IDoctypeService _doctypeService;

        public HomeController(ILogger<HomeController> logger, IDocumentService documentService, IDoctypeService doctypeService)
        {
            _logger = logger;
            _documentService = documentService;
            _doctypeService = doctypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(Document document)
        {
            await _documentService.SaveDocument(document).ConfigureAwait(false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _documentService.GetDocumentById(id.ToString()).ConfigureAwait(false);

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Document document)
        {
            await _documentService.SaveDocument(document).ConfigureAwait(false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> Delete(int id)
        {
            await _documentService.DeleteDocument(id.ToString()).ConfigureAwait(false);

            return Json(new { status = "success", message = "Delete success !" });
        }

        public async Task<JsonResult> GetListMenu()
        {
            var lstMenu = await _doctypeService.GetListDocType().ConfigureAwait(false);
            return Json(new { status = "success", message = "Delete success !", ListMenu = lstMenu });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
