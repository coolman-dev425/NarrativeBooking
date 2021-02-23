using WebApplication1.DBModel;
using System;

namespace WebApplication1.Identity
{
    public class Branch : BaseModel
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        
        /// <summary>
        /// start date of this branch to be able use this application
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// end date of this branch to be able use this application
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Package type of this branch
        /// to determine duration of subscriptions
        /// </summary>
        public int MstPackageId { get; set; }
    }
}
