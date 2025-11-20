using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Tag : Entity  
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        
        // Navigation property
        public List<BlogTag> BlogTags { get; set; }
    }
}