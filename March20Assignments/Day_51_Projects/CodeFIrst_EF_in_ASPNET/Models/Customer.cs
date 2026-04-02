using System.ComponentModel.DataAnnotations;

namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
