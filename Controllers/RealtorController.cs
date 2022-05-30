using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    public class RealtorController : ControllerBase
    {
        private readonly IRealtorService _realtorService;
        
        public RealtorController(IRealtorService realtorService)
        {
            _realtorService = realtorService;
        }
        [HttpPost("RealtorSignUp")]
        public async Task<IActionResult> RegisterRealtor(RealtorRequestModel model)
        {
            var addRealtor = await _realtorService.RegisterRealtor(model);
            if (addRealtor.Status)
            {
                return Ok(addRealtor.Message);
            }

            return BadRequest(addRealtor.Message);
        }
        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddProperty(PropertyRequestModel model)
        {
            var realtorId = 1;
            var addProperty = await _realtorService.AddProperty(model,realtorId);
            if (addProperty.Status)
            {
                return Ok(addProperty.Data.ImagePath);
            }
            return BadRequest(addProperty.Message);
        }
        
        [HttpPatch("UpdateProfile")]
        public async Task<IActionResult> UpDateProfile(RealtorUpdateRequest request)
        {
            var realtorId = 2;
            var update = await _realtorService.UpdateRealtorInfo(request, realtorId);
            if (update.Status)
            {
                return Ok(update.Message);
            }

            return BadRequest(update.Message);
        }
        [HttpGet("RealtorProperty")]
        public IActionResult GetRealtorProperties()
        {
            var realtorId = 2;
            var realtorProperty =  _realtorService.GetPropertyByRealtorId(realtorId);
            if (realtorProperty.Status)
            {
                return Ok(realtorProperty.Data);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet("ApprovedProperty")]
        public IActionResult RealtorApprovedProperties()
        {
            var realtorId = 2;
            var realtorProperty =  _realtorService.GetRealtorApprovedProperty(realtorId);
            if (realtorProperty.Status)
            {
                return Ok(realtorProperty.Data);
            }
            else
            {
                return NoContent();
            }
        }

        public IActionResult RealtorySoldProperties()
        {
            var realtorId = 2;
            var realtorProperty = _realtorService.GetSoldPropertyByRealtor(realtorId);
            if (realtorProperty.Status)
            {
                return Ok(realtorProperty.Data);
            }
            else
            {
                return NoContent();
            }
        }
    }
}