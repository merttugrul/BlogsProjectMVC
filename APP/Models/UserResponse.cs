using APP.Domain;
using CORE.APP.Models;
using System;
using System.ComponentModel;

namespace APP.Models
{
    public class UserResponse : Response
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        public Genders Gender { get; set; }
        
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }
        
        [DisplayName("Registration Date")]
        public DateTime RegistrationDate { get; set; }
        
        public decimal Score { get; set; }
        
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        
        public string Address { get; set; }
        
        public int? CountryId { get; set; }
        
        public int? CityId { get; set; }
        
        public int? GroupId { get; set; }
        
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        
        [DisplayName("Birth Date")]
        public string BirthDateF { get; set; }
        
        [DisplayName("Gender")]
        public string GenderF { get; set; }
        
        [DisplayName("Registration Date")]
        public string RegistrationDateF { get; set; }
        
        [DisplayName("Score")]
        public string ScoreF { get; set; }
        
        [DisplayName("Is Active")]
        public string IsActiveF { get; set; }
        
        [DisplayName("Group")]
        public string GroupTitle { get; set; }
        
        [DisplayName("Roles")]
        public string RoleNames { get; set; }
    }
}