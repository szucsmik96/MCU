using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU.DTO
{
    public class FilmSuperheroDto
    {
        public int? FilmsuperheroId { get; set; }

        public int FilmId { get; set; }
        public int SuperheroId { get; set; }
    }
}