using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Identity
{
    public class User : IdentityUser<int>
    {
        public User()
        {
        }

        public int Status { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string PhoneHome
        {
            get;
            set;
        }

        [StringLength(100)]
        public string PhoneOffice
        {
            get;
            set;
        }

        [StringLength(100)]
        public string CardType
        {
            get;
            set;
        }

        [StringLength(100)]
        public string CardNumber
        {
            get;
            set;
        }

        [StringLength(250)]
        public string AddressHome
        {
            get;
            set;
        }

        [StringLength(100)]
        public string NameOffice
        {
            get;
            set;
        }

        [StringLength(100)]
        public string Department
        {
            get;
            set;
        }

        [StringLength(100)]
        public string JobTitle
        {
            get;
            set;
        }

        [StringLength(100)]
        public string Fax
        {
            get;
            set;
        }

        [StringLength(250)]
        public string AddressOffice
        {
            get;
            set;
        }

        [StringLength(100)]
        public string CityOffice
        {
            get;
            set;
        }

        [StringLength(100)]
        public string CountryOffice
        {
            get;
            set;
        }

        [StringLength(100)]
        public string StatusUser
        {
            get;
            set;
        }

        public string UserToken
        {
            get;
            set;
        }

        public string DebitCardNo { get; set; }
        public int DebitCardMonth { get; set; }
        public int DebitCardYear { get; set; }

        public string StripeCustomerId { get; set; }
        public string StripePersonId { get; set; }
        public string StripeAccountId { get; set; }
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public int? TokenLimit { get; set; }

        public int? TokenUsage { get; set; }

        public bool? IsOnline { get; set; }
        public FollowType FollowType { get; set; }
        [NotMapped]
        public bool HasAvatar { get; set; }
    }

    public enum FollowType
    {
        Follower = 0,
        Following = 1,
        MutualFollow = 2
    }
}
