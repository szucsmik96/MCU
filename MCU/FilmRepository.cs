using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MCU.DTO;
using MCU.Models;

namespace MCU
{
    public class FilmRepository : IFilmRepository
    {
        private ApplicationDbContext dbContext;
        private IMapper mapper;

        public FilmRepository()
        {
            dbContext = new ApplicationDbContext();

            var mapperConfiguration = new MapperConfiguration(c => 
            {
                c.CreateMap<Film, FilmDto>();
                c.CreateMap<FilmDto, Film>();
            });

            mapper = new Mapper(mapperConfiguration);
        }

        public IEnumerable<FilmDto> GetAll()
        {
            var result = new List<FilmDto>();

            List<Film> filmEntities = dbContext.Films.ToList();

            foreach (Film filmEntity in filmEntities)
            {
                result.Add(mapper.Map<FilmDto>(filmEntity));
            }

            return result;
        }

        public FilmDto GetById(int id)
        {
            FilmDto result = null;

            Film filmEntity = dbContext.Films.Find(id);

            if (filmEntity != null)
            {
                result = mapper.Map<FilmDto>(filmEntity);
            }

            return result;
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        public FilmDto CreateOrUpdate(FilmDto film)
        {
            FilmDto result;
            Film filmEntity;

            if (!film.FilmId.HasValue)
            {
                filmEntity = new Film();
                dbContext.Films.Add(filmEntity);
            }
            else
            {
                filmEntity = dbContext.Films.Find(film.FilmId);
            }

            mapper.Map(film, filmEntity);

            dbContext.SaveChanges();

            result = mapper.Map<FilmDto>(filmEntity);

            return result;
        }

        public bool DeleteById(int id)
        {
            bool result = false;

            Film filmEntity = dbContext.Films.Find(id);

            if (filmEntity != null)
            {
                dbContext.Films.Remove(filmEntity);

                result = true;
            }
            
            dbContext.SaveChanges();

            return result;
        }
    }
}