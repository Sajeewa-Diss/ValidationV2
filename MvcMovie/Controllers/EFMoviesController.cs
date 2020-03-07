using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class EFMoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public EFMoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index0() //note for these examples below to be complete, each method name must have a matched view name.
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies?searchString=hostbust //without a querystring param name defined in the url the param is null e.g. Movies?Ghost doesn't work. It can't guess, even for a single param.
        public async Task<IActionResult> Index1(string searchString) //warning is generated if we make this a nullable type.
        {
            var movies = from m in _context.Movie //this defines a list of all movies in Linq. Note the query is defined but not yet run.
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)); //note in SQLite this query is case sensitive.
            }

            return View(await movies.ToListAsync());
        }

        [HttpPost]  //if we add a POST method signature, this will be used in preference to a default GET method. However, in this case it can't be fully bookmarked.
        public string Index(string searchString, bool notUsed) //second param ias added to make a unique signature. Note the return type is also a simple string.
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Movies/Index/hostbust //now we can now pass the search title as route data (a URL segment) instead of as a query string value! However, you wouldn't do this in practice.
        public async Task<IActionResult> Index2(string id) //here we piggy-back on the default route having an optional paran named id. There is nothing to stop us using it a string type for our purposes.
        {
            var movies = from m in _context.Movie //this defines a list of all movies in Linq. Note the query is defined but not yet run.
                         select m;

            if (!String.IsNullOrEmpty(id))
            {
                movies = movies.Where(s => s.Title.Contains(id)); //note in SQLite this query is case sensitive.
            }

            return View(await movies.ToListAsync());
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie  //a LINQ query that retrieves all the genres from the database.
                                            orderby m.Genre
                                            select m.Genre; //this query doesn't return distinct values, that is done below.

            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5 or movies/details?id=1
        public async Task<IActionResult> Details(int? id)  // (int?) parameter is defined as a nullable type in case no param is provided.
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie); //this line is hit if there are validation errors, so the same screen will be re-dispalyed.
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie) //notice BindAttribute is applied to a param - here it protects against overposting (another option is to use ViewModels).
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id) //a GET delete method should do nothing, as in this case. It just returns the view again.
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) //this method is given a unique name from that of the Get method (vbecase it doesn't have a different param list from the GET delete method).
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
