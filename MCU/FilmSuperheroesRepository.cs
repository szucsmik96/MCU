using AutoMapper;
using MCU.DTO;
using MCU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU
{
    public class FilmSuperheroesRepository : IFilmSuperheroesRepository
    {
        private IMapper mapper;

        private ApplicationDbContext dbContext;

        public FilmSuperheroesRepository()
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<FilmSuperheroes, FilmSuperheroDto>();
                c.CreateMap<FilmSuperheroDto, FilmSuperheroes>();

            });

            mapper = new Mapper(mapperConfiguration);

            dbContext = new ApplicationDbContext();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        public IEnumerable<string> GetSuperheroesByFilm(int filmId)
        {
            List<string> result = dbContext.FilmSuperheroes
                .Where(f => f.FilmId == filmId)
                .Select(fs => fs.Superhero.superheroName).ToList();

            return result;
        }

        public IEnumerable<string> GetFilmsBySuperhero(int superheroId)
        {
            List<string> result = dbContext.FilmSuperheroes
                .Where(s => s.SuperheroId == superheroId)
                .Select(fs => fs.Film.filmName).ToList();

            return result;
        }

        public FilmSuperheroDto CreateOrUpdate(FilmSuperheroDto filmSuperhero)
        {
            FilmSuperheroDto result;
            FilmSuperheroes filmsuperheroEntity;

            filmsuperheroEntity = new FilmSuperheroes();
            dbContext.FilmSuperheroes.Add(filmsuperheroEntity);

            dbContext.SaveChanges();

            result = mapper.Map<FilmSuperheroDto>(filmsuperheroEntity);

            return result;
        }

        public IEnumerable<FilmSuperheroDto> GetAll()
        {
            var result = new List<FilmSuperheroDto>();

            List<FilmSuperheroes> filmSuperheroEntities = dbContext.FilmSuperheroes.ToList();

            foreach (FilmSuperheroes filmSuperheroEntity in filmSuperheroEntities)
            {
                result.Add(mapper.Map<FilmSuperheroDto>(filmSuperheroEntity));
            }

            return result;
        }
    }
}