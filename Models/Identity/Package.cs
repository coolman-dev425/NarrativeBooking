using WebApplication1.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Identity
{
    public class Package : BaseModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Package Duration 
        /// for start date and end date calculation
        /// </summary>
        public int Durations { get; set; }

        /// <summary>
        /// Status of this package
        /// </summary>
        public bool Status { get; set; }
    }
}
