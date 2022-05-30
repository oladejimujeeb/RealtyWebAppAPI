using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    /*[ApiController]*/
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet("AllProperty")]
        public  IActionResult AllProperty()
        {
            var allProperty = _propertyService.AllUnverifiedProperty();
            if (allProperty.Status)
            {
                return Ok(allProperty.Data);
            }

            return BadRequest(allProperty.Message);
        }

        [HttpGet("RealtorProperty")]
        public IActionResult RealtorProperty()
        {
            var realtorId = 1;
            var property = _propertyService.GetPropertyByRealtor(realtorId);
            if (property.Status)
            {
                return Ok(property.Data);
            }

            return BadRequest(property.Message);
        }
    }
}