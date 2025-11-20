using CORE.APP.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Blog : Entity  
    {
        [Required, StringLength(200)]
        public string Title { get; set; }
        
        [Required, StringLength(5000)]
        public string Content { get; set; }
        
        public int? Rating { get; set; }  
        
        public DateTime PublishDate { get; set; }
        
        [Required]
        public int UserId { get; set; } 
        
        // Navigation properties
        public User User { get; set; }  
        public List<BlogTag> BlogTags { get; set; }  
    }
}