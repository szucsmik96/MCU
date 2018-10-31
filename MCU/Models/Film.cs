using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCU.Models
{
    public class Film
    {
        public int? FilmId { get; set; }
        [Required]
        [StringLength(255)]
        public string filmName { get; set; }
        public int year { get; set; }
        public int critics { get; set; }
        public int userRating { get; set; }
        public int income { get; set; }

        public ICollection<FilmSuperheroes> FilmSuperheroes { get; set; }



    }
}