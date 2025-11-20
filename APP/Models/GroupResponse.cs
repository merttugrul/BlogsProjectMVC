using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class GroupResponse : Response
    {
        [DisplayName("Group Title")]
        public string Title { get; set; }
        
        // Formatted field (opsiyonel - şimdilik sadece Title yeterli)
    }
}