using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using KHKTDocs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KHKTDocs.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class RoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;

        public RoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetAllUserRole()
        {
            try
            {
                var listRole = await _userRoleService.GetAllUserRole().ConfigureAwait(false);

                return Json(new { status = "success", message = "success", listRole });
            }
            catch (System.Exception)
            {
                return Json(new { status = "fail", message = "fail" });
            }
        }

        public async Task<JsonResult> SaveUserRole([FromBody]UserRoleModel model)
        {
            try
            {
                var userrole = new apec_khktdocs_userrole
                {
                    id = model.id,
                    username = model.username,
                    isadmin = model.isadmin,
                    isapprove = model.isapprove,
                    isaccess = model.isaccess,
                    isdelete = model.isdelete,
                    issuperadmin = 0
                };
                var result = await _userRoleService.SaveUserRole(userrole);

                if(result != -1)
                {
                    return Json(new { status = "success", message = "success" });
                }
                return Json(new { status = "fail", message = "Không thêm mới cho user đã có quyền" });
            }
            catch (System.Exception)
            {
                return Json(new { status = "fail", message = "fail" });
            }
        }

        public async Task<JsonResult> DeleteUserRole([FromBody]TempModel model)
        {
            try
            {
                await _userRoleService.DeleteUserRole(int.Parse(model.data));
                return Json(new { status = "success", message = "success" });
            }
            catch (System.Exception)
            {
                return Json(new { status = "fail", message = "fail" });
            }
        }
    }
}
