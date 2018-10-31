using AutoMapper;
using MCU.DTO;
using MCU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU
{
    public class SuperheroRepository : ISuperheroRepository
    {
        private ApplicationDbContext dbContext;
        private IMapper mapper;

        public SuperheroRepository()
        {
            dbContext = new ApplicationDbContext();

            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<Superhero, SuperheroDto>();
                c.CreateMap<SuperheroDto, Superhero>();
            });

            mapper = new Mapper(mapperConfiguration);
        }

        public SuperheroDto CreateOrUpdate(SuperheroDto superhero)
        {
            SuperheroDto result;
            Superhero superheroEntity;

            if (!superhero.SuperheroId.HasValue)
            {
                superheroEntity = new Superhero();
                dbContext.Superheroes.Add(superheroEntity);
            }
            else
            {
                superheroEntity = dbContext.Superheroes.Find(superhero.SuperheroId);
            }

            mapper.Map(superhero, superheroEntity);

            dbContext.SaveChanges();

            result = mapper.Map<SuperheroDto>(superheroEntity);

            return result;
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        public IEnumerable<SuperheroDto> GetAll()
        {
            var result = new List<SuperheroDto>();

            List<Superhero> superheroEntities = dbContext.Superheroes.ToList();

            foreach (Superhero superheroEntity in superheroEntities)
            {
                result.Add(mapper.Map<SuperheroDto>(superheroEntity));
            }

            return result;
        }

        public SuperheroDto GetById(int id)
        {
            SuperheroDto result = null;

            Superhero superheroEntity = dbContext.Superheroes.Find(id);

            if (superheroEntity != null)
            {
                result = mapper.Map<SuperheroDto>(superheroEntity);
            }

            return result;
        }

        public bool DeleteById(int id)
        {
            bool result;

            Superhero superheroEntity = dbContext.Superheroes.Find(id);

            if (superheroEntity != null)
            {
                dbContext.Superheroes.Remove(superheroEntity);

                result = true;
            }
            else
            {
                result = false;
            }

            dbContext.SaveChanges();

            return result;
        }
    }
}