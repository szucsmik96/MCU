using MCU.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{
    public interface ISuperheroRepository: IDisposable
    {
        IEnumerable<SuperheroDto> GetAll();

        SuperheroDto GetById(int id);

        SuperheroDto CreateOrUpdate(SuperheroDto superhero);

        bool DeleteById(int id);

    }
}
