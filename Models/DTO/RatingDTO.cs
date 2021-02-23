using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.DTO
{
    public class RatingDTO
    {
        public int ratingvalue { get; set; }
        public long bookid { get; set; }
    }

}
