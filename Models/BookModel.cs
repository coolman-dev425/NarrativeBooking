using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{    
    [Table("BooksTable")]
    public partial class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookFileName { get; set; }
        public string BookAuthor { get; set; }
        public string MaturityRating { get; set; }
        public string Genre1 { get; set; }
        public string Genre2 { get; set; }
        public string WritingType { get; set; }
        public int PageCount { get; set; }
        public string Synopsis { get; set; }
        public string WritingSample { get; set; }
        public string AuthorBio { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BookRowGuid { get; set; }
        public byte[] BookContent { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string CoverPath { get; set; }
        public int? CreatedBy { get; set; }

        public string Editing { get; set; }
        public int RatingTimes { get; set; }
        public int RatingVal { get; set; }

        public int RatingNum { get; set; }
    }
}
