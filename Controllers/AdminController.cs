using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    
    [Route("Api/[controller]")]
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
        
        [HttpGet("getProperty")]
        public async Task<IActionResult> GetPropertyById()
        {
            var propertyId = 1;
            var property = await _adminService.GetPropertyById(propertyId);
            if (property.Status)
            {
                return Ok(property.Data);//getting all imagePath 
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpGet("UnapprovedProperties")]
        public IActionResult AllUnapprovedProperties()
        {
            var properties = _adminService.AllUnverifiedProperty();
            if (properties.Status)
            {
                return Ok(properties.Data);
            }

            return Content("No Data");
        }
        
        [HttpGet("InspectionsRequests")]
        public IActionResult InspectionRequest()
        {
            var getAllInspection = _adminService.VisitationRequest();
            if (getAllInspection.Status)
            {
                return Ok(getAllInspection.Data);
            }

            return BadRequest(getAllInspection.Message);
        }
        
        [HttpPatch("UpdatePropertyForSale")]
        public async Task<IActionResult> UpdatePropertyForSale( int propertyId, UpdatePropertyModel model)
        {
            //int propertyId = 1;
            var update = await _adminService.UpdateRealtorPropertyForSale(propertyId, model);
            if (update.Status)
            {
                return Ok(update.Message);
            }

            return BadRequest(update.Message);
        }

        [HttpGet("SearchByPropertyRegNo")]
        public IActionResult SearchPropertyWithRegNo(SearchRequest searchRequest)
        {
            var result = _adminService.SearchPropertyByRegNo(searchRequest);
            if (result.Status)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}