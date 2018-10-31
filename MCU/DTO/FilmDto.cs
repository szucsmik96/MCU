using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCU.DTO
{
    public class FilmDto
    {
        public int? FilmId { get; set; }
        public string filmName { get; set; }
        public int year { get; set; }
        public int critics { get; set; }
        public int userRating { get; set; }
        public int income { get; set; }
    }
}