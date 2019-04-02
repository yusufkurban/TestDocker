using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroKubernete.Model;

namespace MicroKubernete.Database
{
   public class StoreDataContext : DbContext
   {
      public StoreDataContext(DbContextOptions<StoreDataContext> options)
          : base(options)
      {
      }

      public DbSet<Actor> Actors { get; set; }
   }
}
