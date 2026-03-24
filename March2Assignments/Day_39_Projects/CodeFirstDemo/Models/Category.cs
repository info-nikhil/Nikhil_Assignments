using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo.Models
{
    class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public List<Product> Products { set; get; }

    }
}
