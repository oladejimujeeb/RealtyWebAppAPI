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
                return Ok(addProperty.Data.PropertyRegNo);
            }
            return BadRequest(addProperty.Message);
        }
        
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpDateProfile(RealtorUpdateRequest request)
        {
            var realtorId = 1;
            var update = await _realtorService.UpdateRealtorInfo(request, realtorId);
            if (update.Status)
            {
                return Ok(update.Message);
            }

            return BadRequest(update.Message);
        }
    }
}