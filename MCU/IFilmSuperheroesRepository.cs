using MCU.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{
    public interface IFilmSuperheroesRepository : IDisposable
    {
        IEnumerable<string> GetSuperheroesByFilm(int filmId);
        IEnumerable<string> GetFilmsBySuperhero(int superheroId);
        FilmSuperheroDto CreateOrUpdate(FilmSuperheroDto filmsuperhero);
        IEnumerable<FilmSuperheroDto> GetAll();

    }
}
