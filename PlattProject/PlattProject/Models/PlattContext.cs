using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.Models
{
    public class PlattContext : DbContext
    {
        public PlattContext(DbContextOptions<PlattContext> options)
            : base(options)
        { }

        //public DbSet<Blog> Blogs { get; set; }
        
    }
}
