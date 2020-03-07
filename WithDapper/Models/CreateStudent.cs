using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper.Models
{
    public class CreateStudentMark
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public int Marks { get; set; }
    }
}
