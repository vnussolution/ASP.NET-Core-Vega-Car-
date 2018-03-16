using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vega.Models.City;
using Vega.Models.Library;
using Vega.Models.PieShop;

namespace Vega.Models {
    public class AppDbContext : IdentityDbContext<IdentityUser> {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) {

        }

        public DbSet<Pie> Pies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Models.City.City> Cities { get; set; }
        public DbSet<POI> POI { get; set; }

    }
}