using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    [Route("[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }
        [HttpPost("BuyerSignUp")]
        public async Task<IActionResult> RegisterBuyer(BuyerRequestModel requestModel)
        {
            var signup =await _buyerService.RegisterBuyer(requestModel);
            if (signup.Status)
            {
                return Ok(signup.Message);
            }

            return BadRequest(signup.Message);
        }
        [HttpPost("VisitationRequest")]
        public async Task<IActionResult> VisitationRequest(int propertyId)
        {
            var buyerId = 1;
            var visitation = await _buyerService.MakeVisitationRequest(buyerId, propertyId);
            if (visitation.Status)
            {
                return Ok(visitation.Message);
            }

            return BadRequest(visitation.Message);
        }
        [HttpGet("BuyerProspectiveProperties")]
        public IActionResult BuyerProspectiveProperty()
        {
            var buyerId = 1;
            var getProperties =  _buyerService.GetPropertyByBuyer(buyerId);
            if (getProperties.Status)
            {
                return Ok(getProperties.Data);
            }

            return BadRequest("No Property to display");
        }
    }
}