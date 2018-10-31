using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AutoMapper;
using MCU.DTO;
using Microsoft.SharePoint.Client;

namespace MCU
{
    public class SPSuperheroRepository : ISuperheroRepository
    {
        private IMapper mapper;
        private Web website;
        private ClientContext clientContext;

        public SPSuperheroRepository()
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<ListItem, SuperheroDto>()
                .ForMember(dest => dest.SuperheroId, opt => opt.MapFrom(src => src["ID"]))
                .ForMember(dest => dest.superheroName, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src["description"]))
                .ForMember(dest => dest.actor, opt => opt.MapFrom(src => src["actor"]))
                .ForMember(dest => dest.superpower, opt => opt.MapFrom(src => src["superpower"]))
                .ForMember(dest => dest.side, opt => opt.MapFrom(src => src["side"]));

            });

            mapper = new Mapper(mapperConfiguration);

            NetworkCredential Credentials = new NetworkCredential("slimtest1", "Password1234", "CIT");
            clientContext = new ClientContext("http://sp2013-6/sites/szucsm");
            //clientContext.Credentials = CredentialCache.DefaultCredentials;
            clientContext.Credentials = Credentials;
            website = clientContext.Web;
        }

        public SuperheroDto CreateOrUpdate(SuperheroDto superhero)
        {
            SuperheroDto result;
            ListItem superheroEntity;
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

            if (!superhero.SuperheroId.HasValue)
            {
                superheroEntity = website.Lists.GetByTitle("Superheroes").AddItem(itemCreateInfo);
            }
            else
            {
                superheroEntity = website.Lists.GetByTitle("Superheroes").GetItemById(Convert.ToInt32(superhero.SuperheroId));
            }

            superheroEntity["Title"] = superhero.superheroName;
            superheroEntity["description"] = superhero.description;
            superheroEntity["actor"] = superhero.actor;
            superheroEntity["superpower"] = superhero.superpower;
            superheroEntity["side"] = superhero.side;

            result = mapper.Map<SuperheroDto>(superheroEntity);


            return result;
        }

        public bool DeleteById(int id)
        {
            bool result = false;

            ListItem superheroEntity = website.Lists.GetByTitle("Superheroes").GetItemById(id);

            if (superheroEntity != null)
            {
                superheroEntity.DeleteObject();
                result = true;
            }

            return result;
        }

        public void Dispose()
        {
            clientContext.Dispose();
        }

        public IEnumerable<SuperheroDto> GetAll()
        {
            var result = new List<SuperheroDto>();

            ListItemCollection superheroEntities = website.Lists.GetByTitle("Superheroes").GetItems(CamlQuery.CreateAllItemsQuery());
            clientContext.Load(superheroEntities);
            clientContext.ExecuteQuery();

            foreach (ListItem superheroEntity in superheroEntities)
            {
                result.Add(mapper.Map<SuperheroDto>(superheroEntity));
            }

            return result;
        }

        public SuperheroDto GetById(int id)
        {
            SuperheroDto result = null;

            ListItem superheroEntity = website.Lists.GetByTitle("Superheroes").GetItemById(id);

            if (superheroEntity != null)
            {
                result = mapper.Map<SuperheroDto>(superheroEntity);
            }

            return result;
        }
    }
}