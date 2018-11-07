using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AutoMapper;
using MCU.DTO;
using MCU.Models;
using Microsoft.SharePoint.Client;

namespace MCU
{
    public class SPFilmRepository : IFilmRepository
    {
        private IMapper mapper;
        private Web website;
        private ClientContext clientContext;

        public SPFilmRepository()
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<ListItem, FilmDto>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src["ID"]))
                .ForMember(dest => dest.filmName, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.year, opt => opt.MapFrom(src => src["year"]))
                .ForMember(dest => dest.critics, opt => opt.MapFrom(src => src["critics"]))
                .ForMember(dest => dest.userRating, opt => opt.MapFrom(src => src["userRating"]))
                .ForMember(dest => dest.income, opt => opt.MapFrom(src => src["income"]));

            });

            mapper = new Mapper(mapperConfiguration);

            NetworkCredential Credentials = new NetworkCredential("slimtest1", "Password1234", "CIT");
            clientContext = new ClientContext("http://sp2013-6/sites/szucsm");
            //clientContext.Credentials = CredentialCache.DefaultCredentials;
            clientContext.Credentials = Credentials;
            website = clientContext.Web;
        }

        public FilmDto CreateOrUpdate(FilmDto film)
        {
            FilmDto result;
            ListItem filmEntity;
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

            if (!film.FilmId.HasValue)
            {
                filmEntity = website.Lists.GetByTitle("Films").AddItem(itemCreateInfo);
            }
            else
            {
                filmEntity = website.Lists.GetByTitle("Films").GetItemById(Convert.ToInt32(film.FilmId));
            }

            filmEntity["Title"] = film.filmName;
            filmEntity["year"] = film.year;
            filmEntity["critics"] = film.critics;
            filmEntity["userRating"] = film.userRating;
            filmEntity["income"] = film.income;


            result = mapper.Map<FilmDto>(filmEntity);


            return result;
        }

        public bool DeleteById(int id)
        {
            bool result = false;

            ListItem filmEntity = website.Lists.GetByTitle("Films").GetItemById(id);
            clientContext.Load(filmEntity);
            clientContext.ExecuteQuery();

            if (filmEntity != null)
            {
                filmEntity.DeleteObject();
                result = true;
            }

            return result;
        }

        public void Dispose()
        {
            clientContext.Dispose();
        }

        public IEnumerable<FilmDto> GetAll()
        {
            var result = new List<FilmDto>();

            ListItemCollection filmEntities = website.Lists.GetByTitle("Films").GetItems(CamlQuery.CreateAllItemsQuery());
            clientContext.Load(filmEntities);
            clientContext.ExecuteQuery();

            foreach (ListItem filmEntity in filmEntities)
            {
                result.Add(mapper.Map<FilmDto>(filmEntity));
            }

            return result;
        }

        public FilmDto GetById(int id)
        {
            FilmDto result = null;

            ListItem filmEntity = website.Lists.GetByTitle("Films").GetItemById(id);
            clientContext.Load(filmEntity);
            clientContext.ExecuteQuery();

            if (filmEntity != null)
            {
                result = mapper.Map<FilmDto>(filmEntity);
            }

            return result;
        }
    }
}