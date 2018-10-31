using AutoMapper;
using MCU.DTO;
using MCU.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MCU
{
    public class SPFilmSuperheroesRepository : IFilmSuperheroesRepository
    {
        private IMapper mapper;

        private Web website;
        private ClientContext clientContext;

        public SPFilmSuperheroesRepository()
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<ListItem, FilmSuperheroDto>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.SuperheroId, opt => opt.MapFrom(src => src["SuperheroId"]));

            });

            mapper = new Mapper(mapperConfiguration);
            NetworkCredential Credentials = new NetworkCredential("slimtest1", "Password1234", "CIT");
            clientContext = new ClientContext("http://sp2013-6/sites/szucsm");
            //clientContext.Credentials = CredentialCache.DefaultCredentials;
            clientContext.Credentials = Credentials;
            website = clientContext.Web;
        }

        public void Dispose()
        {
            clientContext.Dispose();
        }

        public IEnumerable<string> GetFilmsBySuperhero(int superheroId)
        {
            List<int> filmIds = website.Lists.GetByTitle("FilmSuperheroes").GetItems(CamlQuery.CreateAllItemsQuery())
                .Where(f => Convert.ToInt32(f["SuperheroId"]) == superheroId)
                .Select(fs => Convert.ToInt32(fs["Title"]))
                .ToList();

            List<string> result = website.Lists.GetByTitle("Films").GetItems(CamlQuery.CreateAllItemsQuery())
                .Where(f => filmIds.Any(id => Convert.ToInt32(f["ID"])== id))
                .Select(a => a["filmName"].ToString())
                .ToList();

            return result;
        }

        public IEnumerable<string> GetSuperheroesByFilm(int filmId)
        {
            List<int> superheroIds = website.Lists.GetByTitle("FilmSuperheroes").GetItems(CamlQuery.CreateAllItemsQuery())
                .Where(f => Convert.ToInt32(f["Title"]) == filmId)
                .Select(fs => Convert.ToInt32(fs["SuperheroId"]))
                .ToList();

            List<string> result = website.Lists.GetByTitle("Superheroes").GetItems(CamlQuery.CreateAllItemsQuery())
                .Where(f => superheroIds.Any(id => Convert.ToInt32(f["ID"]) == id))
                .Select(fs => fs["superheroName"].ToString())
                .ToList();

            return result;
        }

        public FilmSuperheroDto CreateOrUpdate(FilmSuperheroDto filmSuperhero)
        {
            FilmSuperheroDto result;
            ListItem filmSuperheroEntity;
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

            filmSuperheroEntity = website.Lists.GetByTitle("FilmSuperheroes").AddItem(itemCreateInfo);

            filmSuperheroEntity["Title"] = filmSuperhero.FilmId;
            filmSuperheroEntity["SuperheroId"] = filmSuperhero.SuperheroId;

            result = mapper.Map<FilmSuperheroDto>(filmSuperheroEntity);

            return result;
        }

        public IEnumerable<FilmSuperheroDto> GetAll()
        {
            var result = new List<FilmSuperheroDto>();

            ListItemCollection filmSuperheroEntities = website.Lists.GetByTitle("FilmSuperheroes").GetItems(CamlQuery.CreateAllItemsQuery());
            clientContext.Load(filmSuperheroEntities);
            clientContext.ExecuteQuery();

            foreach (ListItem filmSuperheroEntity in filmSuperheroEntities)
            {
                result.Add(mapper.Map<FilmSuperheroDto>(filmSuperheroEntity));
            }

            return result;
        }
    }
}