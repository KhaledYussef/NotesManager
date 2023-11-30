using Core.Domains.Auth;
using Core.Domains.Notes;
using Core.Domains.System;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Note> Notes { get; set; }
        public DbSet<SystemError> SystemErrors { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=notesmanager-server.database.windows.net; Initial Catalog = notesmanager-database; User Id = notesmanager-server-admin; Password = S43PS57AE1AWAPNZ$; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
