using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU.Models
{
    public class Superhero
    {
        public int? SuperheroId { get; set; }
        public string superheroName { get; set; }
        public string description { get; set; }
        public string actor { get; set; }
        public string superpower { get; set; }
        public string side { get; set; }

        public ICollection<FilmSuperheroes> FilmSuperheroes { get; set; }


    }
}