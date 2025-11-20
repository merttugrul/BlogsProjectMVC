using APP.Domain;
using CORE.APP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Gender")]
        public Genders Gender { get; set; }
        
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Registration Date")]
        public DateTime RegistrationDate { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Score")]
        public decimal Score { get; set; }
        
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        
        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Address")]
        public string Address { get; set; }
        
        [DisplayName("Country")]
        public int? CountryId { get; set; }
        
        [DisplayName("City")]
        public int? CityId { get; set; }
        
        [DisplayName("Group")]
        public int? GroupId { get; set; }
        
        [DisplayName("Roles")]
        public List<int> RoleIds { get; set; }
    }
}