using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Implementation.Repositories;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IAdminService
    {
        Task<BaseResponseModel<AdminDto>> RegisterAdmin(AdminRequestModel model);
        Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id);
        BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty();
        BaseResponseModel<IEnumerable<VisitationRequestDto>> VisitationRequest();
    }
}