using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealtyWebApp.DTOs
{
    public class RealtorDto
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string AgentId { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string CacNo { get; set; }
        public string Role { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class RealtorUpdateRequest
    {
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [MinLength(7)]
        [MaxLength(15)]
        public string Password{get;set;}
        /*public string ConfirmPassword{get;set;}*/
        
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        //public IFormFile ProfilePicture { get; set; }
        public string BusinessName { get; set; }
        public string CacNumber { get; set; }
    }
}