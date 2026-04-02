using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public Customer Customer { get; set; }

        [Display(Name = "who buyed")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
    }
}
