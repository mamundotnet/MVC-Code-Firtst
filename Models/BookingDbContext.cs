using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectMVC.Models
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Spot> Spots { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BookingEntry> BookingEntries { get; set; }
    }

}