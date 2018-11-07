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
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = $@"<View>
                                    <Query>
                                        <Where>
                                            <Eq>
                                                <FieldRef Name='SuperheroId'/>
                                                <Value Type='Number'>{superheroId}</Value>
                                            </Eq>
                                        </Where>
                                    </Query>
                                    </View>";

            ListItemCollection filmsuperheroes = website.Lists.GetByTitle("FilmSuperheroes").GetItems(camlQuery);

            clientContext.Load(filmsuperheroes);
            clientContext.ExecuteQuery();

            List<int> filmIds=filmsuperheroes
                //.Where(f => Convert.ToInt32(f["SuperheroId"]) == superheroId)
                .Select(fs => Convert.ToInt32(fs["Title"]))
                .ToList();

            ListItemCollection films = website.Lists.GetByTitle("Films").GetItems(CamlQuery.CreateAllItemsQuery());

            clientContext.Load(films);
            clientContext.ExecuteQuery();

            List<string> result= films.Where(f => filmIds.Any(id => Convert.ToInt32(f.Id)== id))
                .Select(a => a["Title"].ToString())
                .ToList();

            return result;
        }

        public IEnumerable<string> GetSuperheroesByFilm(int filmId)
        {
            ListItemCollection filmsuperheroes = website.Lists.GetByTitle("FilmSuperheroes").GetItems(CamlQuery.CreateAllItemsQuery());

            clientContext.Load(filmsuperheroes);
            clientContext.ExecuteQuery();

            List<int> superheroIds = filmsuperheroes.Where(f => Convert.ToInt32(f["Title"]) == filmId)
                .Select(fs => Convert.ToInt32(fs["SuperheroId"]))
                .ToList();

            ListItemCollection superheroes = website.Lists.GetByTitle("Superheroes").GetItems(CamlQuery.CreateAllItemsQuery());

            clientContext.Load(superheroes);
            clientContext.ExecuteQuery();

            List<string> result = superheroes.Where(f => superheroIds.Any(id => Convert.ToInt32(f.Id) == id))
                .Select(fs => fs["Title"].ToString())
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