using BabyStore.Models.Categories;
using BabyStore.Models.Products;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BabyStore.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext()
            : base("name=StoreContext")
        {
           // Database.CreateIfNotExists();
            //Database.SetInitializer(new DropCreateDatabaseAlways < CodeFirstContext > ());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
 