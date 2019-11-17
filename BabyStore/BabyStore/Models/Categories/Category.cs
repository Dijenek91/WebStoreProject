using BabyStore.Models.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models.Categories
{
    public class Category
    {
        public int ID { get; set; }
        
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}