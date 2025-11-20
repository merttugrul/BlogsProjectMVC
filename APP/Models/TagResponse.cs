using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class TagResponse : Response
    {
        [DisplayName("Tag Name")]
        public string Name { get; set; }
    }
}