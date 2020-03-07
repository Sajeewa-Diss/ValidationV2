using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WithDapper.Models;
using Microsoft.AspNetCore.Http;

namespace WithDapper.Controllers
{
    //[Route("api/[controller]")]
    //[Route("api")]               //qq how to start using this attribute? Both get and post stop working when this is included.
    [ApiController]
    public class FiddlerController : ControllerBase
    {
		private readonly IFiddlerEx _fiddlerRepository;

		public FiddlerController(IFiddlerEx repositoryIn)
		{
			_fiddlerRepository = repositoryIn;
		}

        [HttpGet("api/GetFiddlerEx1")]
        public async Task<ActionResult<string>> GetFiddlerEx1(int id = 1)
        {
            var message = await _fiddlerRepository.DoDapperStuff1(id);

            if (message != null)
            {
                return Ok(message);
            }

            return NotFound(new { Message = $"Something went wrong!!?." });
        }

        [HttpGet("api/GetFiddlerEx2")]
        public async Task<ActionResult<string>> GetFiddlerEx2(int id = 1)
        {
            var message = await _fiddlerRepository.DoDapperStuff2(id);

            if (message != null)
            {
                return Ok(message);
            }

            return NotFound(new { Message = $"Something went wrong!!?." });
        }

        [HttpGet("api/GetMultiMapEx1")]
        public ActionResult<string> GetMultiMapEx1(int id = 1)
        {
            //var message = await _fiddlerRepository.DoDapperStuff3(id);
            var message = _fiddlerRepository.MultiMapEx1(id);

            if (message != null)
            {
                return Ok(message);
            }

            return NotFound(new { Message = $"Something went wrong!!?." });
        }

        [HttpGet("api/GetMultiMapEx2")]
        public async Task<ActionResult<string>> GetMultiMapEx2(int id = 1)
        {
            //var message = await _fiddlerRepository.DoDapperStuff3(id);
            var message = await _fiddlerRepository.MultiMapEx2(id);

            if (message != null)
            {
                return Ok(message);
            }

            return NotFound(new { Message = $"Something went wrong!!?." });
        }

        [HttpGet("api/FiddlerExAddStudentMarksGet")]  // [HttpGet("FiddlerExAddStudentMarksGet")]  also works
        //[ProducesResponseType(StatusCodes.Status201Created)] //qq add 201 created instead?? Status200OK .Status201Created
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult FiddlerExAddStudentMarksGet(string name, string subject, int marks)
        {
            var newItem = new CreateStudentMark { Name = name, Subject = subject, Marks = marks };
            var x = _fiddlerRepository.InsertStudentMarks(newItem);

            if (x != 0)  //(x != null)
            {
                //return Ok(x); 
                return Created(new Uri("/Home/Index/testPathOnly", UriKind.Relative), newItem); //the path is returned in the "Location" header for this item (its value can be nonsense).
                //return CreatedAtAction("FiddlerExAddStudentMarks", newItem); //the full path for this action is returned in the "Location" header for this item (must be valid action method name).
            }

            //return NotFound(new { Message = $"Something went wrong!!?." });
            return BadRequest(new { Message = $"Something went wrong!!?." });
        }

        [HttpPost("api/FiddlerExAddStudentMarksPost")]  // [HttpGet("FiddlerExAddStudentMarksPost")]  doesn't work
        //[ActionName("Post")] //qq is this attribute req'd
        //[ValidateAntiForgeryToken]
        public ActionResult FiddlerExAddStudentMarksPost([Bind("Name, Subject, Marks")] CreateStudentMark sm)
        {
            var x = _fiddlerRepository.InsertStudentMarks(sm);

            if (x != 0)  //(x != null)
            {
                //return Ok(x); 
                //return Created(new Uri("/Home/Index/testPathOnly", UriKind.Relative), newItem); //the path is returned in the "Location" header for this item (its value can be nonsense).
                return CreatedAtAction("FiddlerExAddStudentMarksPost", sm); //the full path for this action is returned in the "Location" header for this item (must be valid action method name).
            }

            //return NotFound(new { Message = $"Something went wrong!!?." });
            return BadRequest(new { Message = $"Something went wrong!!?." });
        }

        //[HttpPost]
        //public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItemDTO todoItemDTO)
        //{
        //    var todoItem = new TodoItem
        //    {
        //        IsComplete = todoItemDTO.IsComplete,
        //        Name = todoItemDTO.Name
        //    };

        //    _context.TodoItems.Add(todoItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(
        //        nameof(GetTodoItem),
        //        new { id = todoItem.Id },
        //        ItemToDTO(todoItem));
        //}

    }
}
