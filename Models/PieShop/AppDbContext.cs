using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vega.Models.PieShop {
    public class AppDbContext : IdentityDbContext<IdentityUser> {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) {

        }

        public DbSet<Pie> Pies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}