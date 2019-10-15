using DreamFood.Common.Models;
using System.Data.Entity;

namespace DreamFood.Domain.Models
{
    public class DataContext:DbContext
    {
        public DataContext() :base("DefaultConnection")
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }

        public System.Data.Entity.DbSet<DreamFood.Common.Models.Recommendation> Recommendations { get; set; }

    }
}
