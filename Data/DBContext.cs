using Microsoft.EntityFrameworkCore;
using ASP_MVC_Prueba.Models;

namespace ASP_MVC_Prueba.Data;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {

    }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Employe> Employees { get; set; }
}

