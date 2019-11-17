using BabyStore.Models.Categories;
using BabyStore.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.DAL
{
    public class DataInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
             var categories = new List<Category>
            {
                new Category{ID=1,Name="Toys"},
                new Category{ID=2,Name="Uncategorized"}
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product{ID=1,Name="Alexander",Description="aleksandar veliki", Category=categories[0], CategoryID=1, Price=100},
                new Product{ID=2,Name="Peppa Pig",Description="igracka iz serije", Category=categories[0], CategoryID=2, Price=200},
                new Product{ID=3,Name="Jastuk",Description="mali jastuk", Category=categories[1], CategoryID=1, Price=50},
                new Product{ID=4,Name="Krevetac",Description="krevet za bebe", Category=categories[1], CategoryID=1, Price=654}
            };

            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
           
        }
    }
}