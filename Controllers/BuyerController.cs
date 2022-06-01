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
        [HttpGet("MakeInspectionRequest")]
        public async Task<IActionResult> InspectionRequest(int propertyId)
        {
            var buyerId = 1;
            var visitation = await _buyerService.MakeVisitationRequest(buyerId, propertyId);
            if (visitation.Status)
            {
                return Ok(visitation.Message);
            }

            return BadRequest(visitation.Message);
        }
        [HttpGet("PurchasedProperties")]
        public IActionResult BuyerPurchasedProperty()
        {
            var buyerId = 1;
            var getProperties =  _buyerService.GetPropertyByBuyer(buyerId);
            if (getProperties.Status)
            {
                return Ok(getProperties.Data);
            }

            return BadRequest(getProperties.Message);
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadPropertyDocument(int propertyId)
        {
            var file = await _buyerService.DownloadPropertyDocument(propertyId);
            if (file.Status)
            {
                return File(file.Data.Data, file.Data.FileType, file.Data.DocumentPath + file.Data.Extension);
                //return File(Url.Content("~/Files/text.txt"), "text/plain", "testFile.txt");
            }

            return BadRequest(file.Message);
        }

        [HttpGet("propertyInspection")]
        public IActionResult BuyerPropertyInspection()
        {
            int buyerId = 1;
            var propertyInspection = _buyerService.ListOfRequestedProperties(buyerId);
            if (propertyInspection.Status)
            {
                return Ok(propertyInspection.Data);
            }

            return BadRequest(propertyInspection.Message);
        }
    }
}