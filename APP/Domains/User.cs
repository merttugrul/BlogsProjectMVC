using CORE.APP.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class User : Entity
    {
        [Required, StringLength(50)]
        public string UserName { get; set; }
        
        [Required, StringLength(50)]
        public string Password { get; set; }
        
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }
        
        public Genders Gender { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public DateTime RegistrationDate { get; set; }
        
        public decimal Score { get; set; }
        
        public bool IsActive { get; set; }
        
        [StringLength(500)]
        public string Address { get; set; }
        
        public int? CountryId { get; set; }
        
        public int? CityId { get; set; }
        
        public int? GroupId { get; set; }
        
        public Group Group { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}