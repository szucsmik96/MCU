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

namespace MCU.Controllers.Api
{
    public class SuperheroesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly ISuperheroRepository superheroRepository;
        private readonly IFilmSuperheroesRepository filmSuperheroesRepository;

        public SuperheroesController(ISuperheroRepository superheroRepository, IFilmSuperheroesRepository filmSuperheroesRepository)
        {
            this.superheroRepository = superheroRepository;
            this.filmSuperheroesRepository = filmSuperheroesRepository;
        }

        // GET: api/Superheroes
        public IEnumerable<SuperheroDto> GetSuperheroes()
        {
            IEnumerable<SuperheroDto> result = superheroRepository.GetAll();

            return result;
        }

        // GET: api/Superheroes/5
        [HttpGet()]
        [ResponseType(typeof(SuperheroDto))]
        public IHttpActionResult GetSuperhero(int id)
        {
            SuperheroDto result = superheroRepository.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [ResponseType(typeof(SuperheroViewModel))]
        public IHttpActionResult GetSuperheroViewModel(int id)
        {
            SuperheroDto superhero;
            List<string> filmNames;

            superhero = superheroRepository.GetById(id);

            if (superhero == null)
            {
                return NotFound();
            }

            filmNames = filmSuperheroesRepository.GetFilmsBySuperhero(id).ToList();


            var result = new SuperheroViewModel
            {
                SuperheroId = superhero.SuperheroId,
                superheroName = superhero.superheroName,
                description = superhero.description,
                actor = superhero.actor,
                superpower = superhero.superpower,
                side = superhero.side,
                films = filmNames
            };

            return Ok(result);
        }

        // PUT: api/Superheroes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSuperhero(int id, Superhero superhero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != superhero.SuperheroId)
            {
                return BadRequest();
            }

            db.Entry(superhero).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuperheroExists(id))
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

        // POST: api/Superheroes
        [ResponseType(typeof(SuperheroDto))]
        public IHttpActionResult PostSuperhero(SuperheroDto superhero)
        {
            SuperheroDto result;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            result = superheroRepository.CreateOrUpdate(superhero);

            return Ok(result);
        }

        // DELETE: api/Superheroes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteSuperhero(int id)
        {
            bool result;

            result = superheroRepository.DeleteById(id);

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
                superheroRepository?.Dispose();
                filmSuperheroesRepository?.Dispose();

                db?.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SuperheroExists(int id)
        {
            return db.Superheroes.Any(e => e.SuperheroId == id);
        }
    }
}