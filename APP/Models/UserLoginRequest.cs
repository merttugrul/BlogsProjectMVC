using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    /// <summary>
    /// Represents a request model for user login operations.
    /// Contains the credentials required for authentication.
    /// </summary>
    public class UserLoginRequest
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
    }
}