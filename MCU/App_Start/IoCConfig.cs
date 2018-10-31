using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MCU.App_Start
{
    public class IoCConfig
    {
        public static void Configure()
        {
            var configuration = GlobalConfiguration.Configuration;

            var containerBuilder = new ContainerBuilder();
            
            //containerBuilder.RegisterType<FilmRepository>().As<IFilmRepository>();
            //containerBuilder.RegisterType<FilmSuperheroesRepository>().As<IFilmSuperheroesRepository>();
            //containerBuilder.RegisterType<SuperheroRepository>().As<ISuperheroRepository>();

            containerBuilder.RegisterType<SPFilmRepository>().As<IFilmRepository>();
            containerBuilder.RegisterType<SPFilmSuperheroesRepository>().As<IFilmSuperheroesRepository>();
            containerBuilder.RegisterType<SPSuperheroRepository>().As<ISuperheroRepository>();

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);

            IContainer container = containerBuilder.Build();            

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}