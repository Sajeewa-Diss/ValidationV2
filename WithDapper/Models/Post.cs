using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
