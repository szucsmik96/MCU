using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU.Models
{
    public class FilmSuperheroes
    {
        public int? FilmsuperheroId { get; set; }

        public int FilmId { get; set; }
        public Film Film { get; set; }

        public int SuperheroId { get; set; }
        public Superhero Superhero { get; set; }

    }
}