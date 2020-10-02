using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using KHKTDocs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KHKTDocs.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Access")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDocumentService _documentService;
        private readonly IDoctypeService _doctypeService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IDocumentService documentService,
            IDoctypeService doctypeService, IUserService userService)
        {
            _logger = logger;
            _documentService = documentService;
            _doctypeService = doctypeService;
            _userService = userService;
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
                string stage = Request.Form["stage"].ToString();
                string doc_description = Request.Form["doc_description"].ToString();
                string create_user = Request.Form["create_user"].ToString();
                string status = Request.Form["status"].ToString();
                string created_date = Request.Form["created_date"].ToString();
                string doc_folder = Request.Form["doc_folder"].ToString();
                string doc_receiver = Request.Form["doc_receiver"].ToString();
                string doc_agency = Request.Form["doc_agency"].ToString();

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
                    stage = stage,
                    document_description = doc_description,
                    document_extension = Path.GetExtension(file.FileName),
                    document_name = Path.GetFileNameWithoutExtension(file.FileName),
                    document_folder_id = int.Parse(doc_folder),
                    created_user = create_user,
                    created_date = Convert.ToDateTime(created_date),
                    status = int.Parse(status),
                    document_receiver = doc_receiver,
                    document_agency = doc_agency
                };
                await _documentService.SaveDocument(document);

                return Json(new { status = "success", message = "success" });
            }
            catch (Exception)
            {
                return Json(new { status = "fail", message = "fail" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _documentService.GetDocumentById(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(apec_khktdocs_document document)
        {
            await _documentService.SaveDocument(document);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, SuperAdmin, Delete")]
        public async Task<JsonResult> Delete([FromBody] TempModel model)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var role = currentUser.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value);

                if (role.Contains("Approve") || role.Contains("Admin") || role.Contains("SuperAdmin"))
                {
                    var res = await _documentService.GetDocumentById(int.Parse(model.data));
                    string fileName = res.document_name + res.document_extension;
                    var fullpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", fileName);

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }

                    await _documentService.DeleteDocument(int.Parse(model.data));
                    return Json(new { status = "success", message = "Delete success !" });
                }
                return Json(new { status = "fail", message = " Xoá không thành công! \n Không có quyền Xoá!" });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = " Xoá không thành công! \n Có lỗi xảy ra " });
            }
        }

        public async Task<JsonResult> GetListMenu()
        {
            var lstMenu = await _doctypeService.GetListDocType();
            return Json(new { status = "success", message = "success !", ListMenu = lstMenu });
        }

        public async Task<JsonResult> GetListDocuments()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var role = currentUser.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value);

            var listDocs = await _documentService.GetAllDocument();

            return Json(new { status = "success", message = "success !", listDocs, role });
        }
        public async Task<JsonResult> SearchDocsByFolderId(TempModel model)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var role = currentUser.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value);
                var listDocs = await _documentService.GetDocsByFolderId(model.data);

                return Json(new { status = "success", message = "success !", listDocs, role });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = e });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SearchByConditions([FromBody] SearchConditionsDTO model)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var role = currentUser.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value);
                var listDocs = await _documentService.GetDocsByConditions(model);
                return Json(new { status = "success", message = "success !", listDocs, role });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = e });
            }
        }

        public async Task<JsonResult> FolderEvents(FolderViewModel folderViewModel)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var username = currentUser.Claims.First(x => x.Type == "UserName").Value;

                apec_khktdocs_folder folder = new apec_khktdocs_folder();

                if (folderViewModel.action == "Create")
                {
                    folder = new apec_khktdocs_folder
                    {
                        parent = folderViewModel.parent,
                        text = folderViewModel.text,
                        created_user = username
                    };
                    var result = await _doctypeService.SaveFolder(folder);
                    return Json(new { status = "success", message = "success !", result });
                }
                else if (folderViewModel.action == "Rename")
                {
                    folder = new apec_khktdocs_folder
                    {
                        id = int.Parse(folderViewModel.id),
                        parent = folderViewModel.parent,
                        text = folderViewModel.text,
                        modified_user = username
                    };
                    await _doctypeService.SaveFolder(folder);
                    return Json(new { status = "success", message = "success !" });
                }
                else
                {
                    await _doctypeService.DeleteFolder(int.Parse(folderViewModel.id));
                    return Json(new { status = "success", message = "success !" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = "fail !" });
            }
        }

        public async Task<JsonResult> BindDataSelect()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var username = currentUser.Claims.First(x => x.Type == "UserName").Value;
            var lstUser = await _userService.GetAllUserWithDepart();

            return Json(new { status = "success", message = "success !", lstUser, username });
        }

        [Authorize(Roles = "Approve, Admin, SuperAdmin")]
        public async Task<JsonResult> ApproveDoc([FromBody] TempModel model)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var role = currentUser.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value);

                if (role.Contains("Approve") || role.Contains("Admin") || role.Contains("SuperAdmin"))
                {
                    await _documentService.ApproveDocument(int.Parse(model.data));
                    return Json(new { status = "success", message = "success !" });
                }
                return Json(new { status = "fail", message = " Duyệt không thành công! \n Duyệt có quyền Xoá!" });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = e });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var res = await _documentService.GetDocumentById(id);
            string fileName = res.document_name + res.document_extension;
            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", fileName);
            var fs = new FileStream(fullpath, FileMode.Open);

            return File(fs, "application/octet-stream", fileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
