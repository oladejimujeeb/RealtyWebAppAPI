using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class BuyerService:IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;

        public BuyerService(IBuyerRepository buyerRepository, IRoleRepository roleRepository, IUserRepository userRepository, IPropertyRepository propertyRepository, IVisitationRepository visitationRepository)
        {
            _buyerRepository = buyerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
        }
        public async Task<BaseResponseModel<BuyerDto>> RegisterBuyer(BuyerRequestModel model)
        {
            var checkMail = await _userRepository.Exists(x => x.Email == model.Email);
            if (checkMail)
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Message = "Email Already Exist",
                    Status = false
                };
            }
            var role = RoleConstant.Buyer.ToString();
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
                //User = user,
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

            var buyer = new Buyer()
            {
                Address = model.Address,
                User = user,
                UserId = user.Id,
                RegId = $"CLT/{Guid.NewGuid().ToString().Substring(0, 4)}",
            };
            var addBuyer = await _buyerRepository.Add(buyer);
            if (addBuyer != buyer)//potential error
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Status = false,
                    Message = "Registration  failed"
                };
            }
            return new BaseResponseModel<BuyerDto>()
            {
                Status = true,
                Message = "Registration  Successful",
                Data = new BuyerDto()
                {
                    RegNo = addBuyer.RegId
                }
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByBuyer(int buyerId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.BuyerIdentity == buyerId && x.IsAvailable).
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
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    ImagePath = x.PropertyImages.Select(z=>z.DocumentPath).ToList(),//Possible Error
                }).ToList();
            
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public async Task<BaseResponseModel<VisitationRequestDto>> MakeVisitationRequest(int buyerId, int propertyId)
        {
            var getBuyer =await _userRepository.Get(x => x.Id == buyerId);
            var getProperty = await _propertyRepository.Get(x => x.Id == propertyId);
            var request = new VisitationRequest()
            {
                BuyerEmail = getBuyer.Email,
                BuyerId = getBuyer.Id,
                BuyerName = $"{getBuyer.FirstName }{getBuyer.LastName}",
                BuyerTelephone = getBuyer.PhoneNumber,
                PropertyId = getProperty.Id,
                PropertyType = getProperty.PropertyType,
                PropertyRegNo = getProperty.PropertyRegNo,
                Address = getProperty.Address
            };
            var date = DateTime.Now.AddDays(3);
            
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
               var newDate = date.AddDays(1);
               request.RequestDate = newDate;
               
            }
            else
            {
                request.RequestDate = date;
            }

            var addVisitation =await _visitationRepository.Add(request);
            if (addVisitation != request)
            {
                return new BaseResponseModel<VisitationRequestDto>()
                {
                    Status = false,
                    Message = "Fail to Schedule date called Customer Service",
                };
            }
            var scheduledDate = date;
            string visitDate = scheduledDate.ToString("dddd,dd MMMM yyyy");
            return new BaseResponseModel<VisitationRequestDto>()
            {
                
                Status = true,
                Message = $"Kind Visit Our office on {visitDate} for Property Inspection," +
                          $" If Date is not convenient call Our Customer Service on 08136794915 to reschedule ",
            };
        }
    }
}