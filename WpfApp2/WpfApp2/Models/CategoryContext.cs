using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApp2.Models
{
    public class CategoryContext : DbContext
    {
        public CategoryContext() : base("DefaultConnection")
        {

        }

        public DbSet<Category> Categories { get; set; }
    }
}
