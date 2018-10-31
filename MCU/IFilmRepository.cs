using MCU.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{
    public interface IFilmRepository : IDisposable
    {
        IEnumerable<FilmDto> GetAll();

        FilmDto GetById(int id);

        FilmDto CreateOrUpdate(FilmDto film);

        bool DeleteById(int id);

    }
}
