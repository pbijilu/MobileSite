using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MobileSite.Core;

namespace MobileSite.Data
{
    public class MobileSiteDbContext : DbContext
    {
        public MobileSiteDbContext(DbContextOptions<MobileSiteDbContext> options) : base (options)
        {

        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Good> Goods { get; set; }
    }
}
