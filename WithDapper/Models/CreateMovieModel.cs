using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class CreateMovieModel
    {
        public string Name { get; set; }
        public int DirectorId { get; set; }
        public short ReleaseYear { get; set; }
    }
}
