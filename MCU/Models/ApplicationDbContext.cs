using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MCU.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Superhero> Superheroes { get; set; }
        public DbSet<FilmSuperheroes> FilmSuperheroes { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FilmSuperheroes>().HasRequired(a => a.Film).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<FilmSuperheroes>().HasKey(a => new { a.FilmId, a.SuperheroId });
            modelBuilder.Entity<FilmSuperheroes>().HasRequired<Film>(a => a.Film).WithMany(a => a.FilmSuperheroes).HasForeignKey<int>(s => s.FilmId);
            modelBuilder.Entity<FilmSuperheroes>().HasRequired<Superhero>(a => a.Superhero).WithMany(a => a.FilmSuperheroes).HasForeignKey<int>(s => s.SuperheroId);

            base.OnModelCreating(modelBuilder);
        }
    }
}