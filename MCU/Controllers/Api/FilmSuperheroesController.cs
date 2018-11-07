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

namespace MCU.Controllers.Api
{
    public class FilmSuperheroesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly IFilmRepository filmRepository;
        private readonly ISuperheroRepository superheroRepository;
        private readonly IFilmSuperheroesRepository filmSuperheroesRepository;

        public FilmSuperheroesController(IFilmRepository filmRepository, ISuperheroRepository superheroRepository, IFilmSuperheroesRepository filmSuperheroesRepository)
        {
            this.filmRepository = filmRepository;
            this.superheroRepository = superheroRepository;
            this.filmSuperheroesRepository = filmSuperheroesRepository;
        }


        // GET: api/FilmSuperheroes/5
        public IEnumerable<FilmSuperheroDto> GetFilmSuperheroes()
        {
            IEnumerable<FilmSuperheroDto> result = filmSuperheroesRepository.GetAll();

            return result;
        }


        // POST: api/FilmSuperheroes
        [ResponseType(typeof(FilmSuperheroes))]
        public IHttpActionResult PostFilmSuperheroes(FilmSuperheroDto filmSuperheroes)
        {
            FilmSuperheroDto result;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            result = filmSuperheroesRepository.CreateOrUpdate(filmSuperheroes);

            return Ok(result);
        }

        // DELETE: api/FilmSuperheroes/5
        [ResponseType(typeof(FilmSuperheroes))]
        public async Task<IHttpActionResult> DeleteFilmSuperheroes(int id)
        {
            FilmSuperheroes filmSuperheroes = await db.FilmSuperheroes.FindAsync(id);
            if (filmSuperheroes == null)
            {
                return NotFound();
            }

            db.FilmSuperheroes.Remove(filmSuperheroes);
            await db.SaveChangesAsync();

            return Ok(filmSuperheroes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                filmRepository?.Dispose();
                superheroRepository?.Dispose();
                filmSuperheroesRepository?.Dispose();

                db?.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FilmSuperheroesExists(int id)
        {
            return db.FilmSuperheroes.Count(e => e.FilmId == id) > 0;
        }
    }
}