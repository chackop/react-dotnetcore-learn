using Microsoft.EntityFrameworkCore;
using dotNetRestWebApp.Models;

namespace dotNetRestWebApp
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<SessionRec> SessionRecs { get; set; }

        public DbSet<SpeakerRec> SpeakerRecs { get; set; }

    }
}
