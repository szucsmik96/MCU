using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MCU.DTO;
using MCU.Models;
using MCU.ViewModels;
//using MCU.Repositories;

namespace MCU.Controllers.Api
{
    public class FilmsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly IFilmRepository filmRepository;
        private readonly IFilmSuperheroesRepository filmSuperheroesRepository;

        public FilmsController(IFilmRepository filmRepository, IFilmSuperheroesRepository filmSuperheroesRepository)
        {
            this.filmRepository = filmRepository;
            this.filmSuperheroesRepository = filmSuperheroesRepository;
        }

        // GET: api/Films
        public IEnumerable<FilmDto> GetFilms()
        {
            IEnumerable<FilmDto> result = filmRepository.GetAll();
            
            return result;
        }

        // GET: api/Films/5
        [HttpGet()]
        [ResponseType(typeof(FilmDto))]
        public IHttpActionResult GetFilm(int id)
        {
            FilmDto result = filmRepository.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [ResponseType(typeof(FilmViewModel))]
        public IHttpActionResult GetFilmViewModel(int id)
        {
            FilmDto film;
            List<string> superheroNames;

            film = filmRepository.GetById(id);
            
            if (film == null)
            {
                return NotFound();
            }

            superheroNames = filmSuperheroesRepository.GetSuperheroesByFilm(id).ToList();
            
            var result = new FilmViewModel
            {
                FilmId = film.FilmId,
                critics = film.critics,
                filmName = film.filmName,
                income = film.income,
                userRating = film.userRating,
                year = film.year,
                superheroes = superheroNames
            };

            return Ok(result);
        }

        // PUT: api/Films/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFilm(int id, Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != film.FilmId)
            {
                return BadRequest();
            }
            
            db.Entry(film).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Films
        [ResponseType(typeof(FilmDto))]
        public IHttpActionResult PostFilm(FilmDto film)
        {
            FilmDto result;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            result = filmRepository.CreateOrUpdate(film);

            return Ok(result);
        }

        // DELETE: api/Films/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteFilm(int id)
        {
            bool result;

            result=filmRepository.DeleteById(id);

            if (result == false)
            {
                return NotFound();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                filmRepository?.Dispose();
                filmSuperheroesRepository?.Dispose();

                db?.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FilmExists(int id)
        {
            return db.Films.Count(e => e.FilmId == id) > 0;
        }
    }
}