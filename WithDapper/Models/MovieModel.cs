using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class MovieModel
    {
        public int Id { get; set; }  //If the column names differ we can use SQL aliases to match with a property name for use by dapper.
        public string Name { get; set; }
        public string DirectorName { get; set; }
        public short ReleaseYear { get; set; }
    }
}
