using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/
        //public string Index()
        //{
        //    return "This is my default action...";
        //}

        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /HelloWorld/Welcome/                        //this method can't coexist with the method directly below if it has same name. Error: Multiple endpoints not allowed.
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        // GET: /HelloWorld/Welcome2?name=Rick&numtimes=4 or /HelloWorld/Welcome2?numtimes=3&name=Rick the params can be in any order
        // Requires using System.Text.Encodings.Web;
        public string Welcome2(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        //GET /HelloWorld/Welcome3/101?name=Rick    //the param "101" automatically replaces the id param via the routing setting. Note ID/id is case-sensitive in C# but case-insensitive in URL.
        // without the default routing the same result is returned via the following GET request  /HelloWorld/Welcome3/?id=101&name=Rick (and here the final / is optional)
        public string Welcome3(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }   //Here the controller is returning HTML directly. Generally you don't want controllers returning HTML directly

        //GET /HelloWorld/Welcome4?name=Rick&numtimes=4
        public IActionResult Welcome4(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}
