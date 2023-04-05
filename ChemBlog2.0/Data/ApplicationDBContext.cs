using ChemBlog2._0.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChemBlog2._0.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<AdminPanelEntity> AddArticles { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }    
    }
}
