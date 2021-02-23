using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CustomerBooks
    {
        [Key]
        public long BookId { get; set; }
        public string CustomerId { get; set; }
        public bool IsStripeUser { get; set; }
    }
}
