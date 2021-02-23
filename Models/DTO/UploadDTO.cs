using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.DTO
{
    public class UploadDTO
    {
        public string book_title { get; set; }
        public IFormFile docFile { get; set; }
        public string book_price { get; set; }
        public string Genre1 { get; set; }
        public string Genre2 { get; set; }
        public string WritingType { get; set; }
        public string Synopsis { get; set; }
        public int PageCount { get; set; }
        public string BookAuthor { get; set; }
        public string AuthorBio { get; set; }
        public string WritingSample { get; set; }
        public string stripeName { get; set; }
        public string stripeEmail { get; set; }
        public string paymentIntentId { get; set; }
        public string debitCardNo { get; set; }
        public string Editing { get; set; }

    }

}
