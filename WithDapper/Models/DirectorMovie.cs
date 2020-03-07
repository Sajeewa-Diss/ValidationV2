using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class DirectorMovie
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public short ReleaseYear { get; set; }
    }
}
