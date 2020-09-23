using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using KHKTDocs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateDoc()
        {
            try
            {
                IFormFile file = Request.Form.Files.First();
                string document_name = Request.Form["document_name"].ToString();
                string doc_description = Request.Form["doc_description"].ToString();
                string create_user = Request.Form["create_user"].ToString();
                string status = Request.Form["status"].ToString();
                string created_date = Request.Form["created_date"].ToString();
                string doc_folder = Request.Form["doc_folder"].ToString();

                var memoryStream = new MemoryStream();
                file.OpenReadStream().CopyTo(memoryStream);
                var fileData = memoryStream.ToArray();
                var fullpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", file.FileName);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                System.IO.File.WriteAllBytes(fullpath, fileData);

                apec_khktdocs_document document = new apec_khktdocs_document
                {
                    document_name = document_name,
                    document_description = doc_description,
                    document_extension = Path.GetExtension(file.FileName),
                    display_name = Path.GetFileNameWithoutExtension(file.FileName),
                    document_folder_id = 11,//int.Parse(doc_folder),
                    created_user = create_user,
                    created_date = Convert.ToDateTime(created_date),
                    status = status
                };
                await _documentService.SaveDocument(document).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return Json(new { status = "fail", message = "fail" });
            }
            return Json(new { status = "success", message = "success" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _documentService.GetDocumentById(id).ConfigureAwait(false);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(apec_khktdocs_document document)
        {
            await _documentService.SaveDocument(document).ConfigureAwait(false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> Delete(int id)
        {
            await _documentService.DeleteDocument(id).ConfigureAwait(false);

            return Json(new { status = "success", message = "Delete success !" });
        }

        public async Task<JsonResult> GetListMenu()
        {
            var lstMenu = await _doctypeService.GetListDocType().ConfigureAwait(false);
            return Json(new { status = "success", message = "success !", ListMenu = lstMenu});
        }

        public async Task<JsonResult> GetListDocuments()
        {
            var lstDocs = await _documentService.GetAllDocument().ConfigureAwait(false);

            return Json(new { status = "success", message = "success !", ListDocs = lstDocs });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
