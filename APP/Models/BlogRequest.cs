using CORE.APP.Models;  
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class BlogRequest : Request  
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Title")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(5000, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Content")]
        public string Content { get; set; }
        
        [DisplayName("Rating")]
        [Range(1, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? Rating { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }
        
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("User")]
        public int UserId { get; set; }
        
        [DisplayName("Tags")]
        public List<int> TagIds { get; set; }  
    }
}