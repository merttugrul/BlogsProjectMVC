using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    /// <summary>
    /// Represents a request model for user registration operations.
    /// Contains the information required to create a new user account.
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        /// Gets or sets the user name.
        /// This field is required, therefore can't be null.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the password.
        /// This field is required, therefore can't be null.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        /// <summary>
        /// Gets or sets the password confirmation.
        /// This field is required and must match the Password field.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}