using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WithDapper.Models;

namespace WithDapper
{
    public interface IFiddlerEx
    {
        Task<string> DoDapperStuff1(int id);

        Task<string> DoDapperStuff2(int id);

        //Task<string> DoDapperStuff3(int id);
        string MultiMapEx1(int id);

        Task<string> MultiMapEx2(int id);

        int InsertStudentMarks(CreateStudentMark student); //qq can this be a student which gets an id set for it??
    }
}
