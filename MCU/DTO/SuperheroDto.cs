using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU.DTO
{
    public class SuperheroDto
    {
        public int? SuperheroId { get; set; }
        public string superheroName { get; set; }
        public string description { get; set; }
        public string actor { get; set; }
        public string superpower { get; set; }
        public string side { get; set; }
    }
}