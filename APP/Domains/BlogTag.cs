using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class BlogTag : Entity  
    {
        [Required]
        public int BlogId { get; set; }
        
        [Required]
        public int TagId { get; set; }
        
        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}