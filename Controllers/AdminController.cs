using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost("SignUp")]
        public  async Task<IActionResult> RegisterAdmin(AdminRequestModel model)
        {
            var addAdmin = await _adminService.RegisterAdmin(model);
            if (addAdmin.Status)
            {
                return Ok(addAdmin.Message);
            }
            return BadRequest("Registration failed");
        }
        
    }
}