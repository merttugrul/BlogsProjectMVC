using CORE.APP.Models;  
using System;
using System.ComponentModel;

namespace APP.Models
{
    public class BlogResponse : Response  
    {
        [DisplayName("Title")]
        public string Title { get; set; }
        
        [DisplayName("Content")]
        public string Content { get; set; }
        
        [DisplayName("Rating")]
        public int? Rating { get; set; }
        
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }
        
        public int UserId { get; set; }
        
        [DisplayName("Publish Date")]
        public string PublishDateF { get; set; }
        
        [DisplayName("Rating")]
        public string RatingF { get; set; }
        
        [DisplayName("Author")]
        public string UserName { get; set; }
        
        [DisplayName("Tags")]
        public string TagNames { get; set; }
    }
}