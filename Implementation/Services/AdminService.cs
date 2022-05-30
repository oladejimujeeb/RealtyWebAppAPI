using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;

        public AdminService(IAdminRepository adminRepository, IRoleRepository roleRepository, IUserRepository userRepository, IPropertyRepository propertyRepository, IVisitationRepository visitationRepository)
        {
            _adminRepository = adminRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
        }   
        public async Task<BaseResponseModel<AdminDto>> RegisterAdmin(AdminRequestModel model)
        {
            var checkMail = await _userRepository.Exists(x => x.Email == model.Email);
            if (checkMail)
            {
                return new BaseResponseModel<AdminDto>()
                {
                    Message = "Email Already Exist",
                    Status = false
                };
            }
            var role = RoleConstant.Administrator.ToString();
            var getRole = await _roleRepository.Get(x => x.RoleName == role);
            
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
            };
            var userRole = new UserRole
            {
                User = user,
                UserId = user.Id,
                RoleId = getRole.Id,
            };
            user.UserRoles.Add(userRole);
            
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\ProfilePictures\\");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists)
            {
                Directory.CreateDirectory(basePath);
            }
            //var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePicture.FileName);
            var extension = Path.GetExtension(model.ProfilePicture.FileName);
            if (!System.IO.File.Exists(filePath)&& extension==".jpg"|| extension ==".png"|| extension==".jpeg")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = filePath;
                await _userRepository.Add(user);
            }

            var admin = new Admin
            {
                Address = model.Address,
                RegId = $"AD/{Guid.NewGuid().ToString().Substring(0, 4)}",
                User = user,
                UserId = user.Id,
            };
            var addAdmin = await _adminRepository.Add(admin);
            return new BaseResponseModel<AdminDto>()
            {
                Status = true,
                Message = "Created Successfully",
                Data = new AdminDto()
                {
                    RegNo = addAdmin.RegId
                }
            };
        }

        public async Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id)
        {
            var getProperty = await _propertyRepository.Get(x => x.Id == id);
            if (getProperty == null)
            {
                return new BaseResponseModel<PropertyDto>
                {
                    Status = false,Message = "Not found",
                };
            }

            return new BaseResponseModel<PropertyDto>
            {
                Status = true,
                Data = new PropertyDto()
                {
                    Id = getProperty.Id,
                    Address = getProperty.Address,
                    Bedroom = getProperty.Bedroom,
                    Features = getProperty.Features,
                    Latitude = getProperty.Latitude,
                    Longitude = getProperty.Longitude,
                    Toilet = getProperty.Toilet,
                    BuildingType = getProperty.BuildingType,
                    BuyerId = getProperty.BuyerIdentity,
                    IsSold = getProperty.IsSold,
                    LandArea = getProperty.PlotArea,
                    PropertyPrice = getProperty.Price,
                    RealtorId = getProperty.RealtorId,
                    PropertyType = getProperty.PropertyType,
                    PropertyRegNumber = getProperty.PropertyRegNo,
                    Action = getProperty.Action,
                    Status = getProperty.Status,
                    VerificationStatus = getProperty.VerificationStatus,
                    IsAvailable = getProperty.IsAvailable,
                    ImagePath = getProperty.PropertyImages.Select(z=>z.DocumentPath).ToList(),
                },
                Message = "load successfully"
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty()
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.BuyerIdentity==0 && x.IsSold==false && x.VerificationStatus==false).
                Select(x=>new PropertyDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Bedroom = x.Bedroom,
                    Features = x.Features,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Toilet = x.Toilet,
                    BuildingType = x.BuildingType,
                    BuyerId = x.BuyerIdentity,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Price,
                    RealtorId = x.RealtorId,
                    PropertyType = x.PropertyType,
                    PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    IsSold = x.IsSold,
                    RegisteredDate = x.RegisteredDate,
                    //ImagePath = x.PropertyImages.Select(z=>z.DocumentPath).ToList(),//Possible Error
                }).OrderBy(x=>x.RegisteredDate).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public BaseResponseModel<IEnumerable<VisitationRequestDto>> VisitationRequest()
        {
            var inspectionRequest =
                _visitationRepository.QueryWhere(x => x.RequestDate < DateTime.Now.AddDays(-4)).
                    Select(x=>new VisitationRequestDto
                    {
                        Id = x.Id,
                        Address = x.Address,
                        BuyerId = x.BuyerId,
                        BuyerName = x.BuyerName,
                        PropertyAddress = x.Address,
                        PropertyId = x.PropertyId,
                        BuyerPhoneNo = x.BuyerTelephone,
                        PropertyType = x.PropertyType,
                        PropertyRegNo = x.PropertyRegNo,
                        RequestDate = x.RequestDate,
                    }).OrderBy(x=>x.RequestDate).ToList();
            return new BaseResponseModel<IEnumerable<VisitationRequestDto>>()
            {
                Status = true,
                Data = inspectionRequest
            };
                    
        }
    }
}