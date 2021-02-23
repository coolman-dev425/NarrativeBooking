using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Purchased")]
    public class Purchased
    {
        public long BookId { get; set; }
        public int CustomerId { get; set; }
        public string BookTitle { get; set; }
    }
}