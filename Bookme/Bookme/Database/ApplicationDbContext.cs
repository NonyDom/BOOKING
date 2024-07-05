using Bookme.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookme.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<BookingForm> BookingForm { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CommonDropDown> CommonDropown { get; set; }
        public DbSet<UserBookings> UserBookings { get; set; }

    }

}

