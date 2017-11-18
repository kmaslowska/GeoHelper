using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GeoHelper.Models;

namespace GeoHelper.Models
{
    public class GeoHelperContext : DbContext
    {
        public GeoHelperContext (DbContextOptions<GeoHelperContext> options)
            : base(options)
        {
        }

        public DbSet<GeoHelper.Models.Point> Point { get; set; }

        public DbSet<GeoHelper.Models.Project> Project { get; set; }

        public DbSet<GeoHelper.Models.UsersProjects> UsersProjects { get; set; }

    }
}
