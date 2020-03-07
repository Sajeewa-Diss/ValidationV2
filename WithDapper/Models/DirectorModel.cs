using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class DirectorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<DirectorMovie> Movies { get; set; }
    }
}
