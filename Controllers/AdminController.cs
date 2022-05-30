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

        public async Task<IActionResult> GetPropertyById()
        {
            var propertyId = 2;
            var property = await _adminService.GetPropertyById(propertyId);
            if (property.Status)
            {
                return Ok(property.Data.ImagePath);//getting all imagePath 
            }
            else
            {
                return NoContent();
            }
        }

        public IActionResult AllUnapprovedProperties()
        {
            var properties = _adminService.AllUnverifiedProperty();
            if (properties.Status)
            {
                return Ok(properties.Data);
            }

            return Content("No Data");
        }

        public IActionResult InspectionRequest()
        {
            var getAllInspection = _adminService.VisitationRequest();
            if (getAllInspection.Status)
            {
                return Ok(getAllInspection.Data);
            }

            return BadRequest("No Info");
        }
    }
}