using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Identity;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WebApplication1.Helper;

namespace WebApplication1.Controllers
{
    public class WishListController : Controller
    {
        public WishList CurrentBook { get; set; }
        public UserManager<User> userManager { get; set; }
        public SignInManager<User> signInManager { get; set; }

        private readonly BooksContext db;

        public WishListController(UserManager<User> userManager, SignInManager<User> signInManager, BooksContext context)
        {
            db = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public ActionResult DetailsWishList()
        {
            //IQueryable<WishList> WishList;
            //WishList = db.WishList;
            //WishList db = new WishList();

            List<WishList> wishlist = db.WishLists.ToList();

            return View(wishlist);
        }

        public IActionResult DetailsWishList(string search = "", string SortColumn = "BookTitle", string SortOrder = "up", int PageNo = 1)
        {
            ViewBag.SortColumn = SortColumn;
            ViewBag.SortOrder = SortOrder;
            ViewBag.search = search;

            IQueryable<WishList> wishList;
            if (search != null)
                wishList = db.WishLists.Join(db.Books, w => w.BookId, b => b.BookId, (w, b) => new WishList
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    CustomerId = w.CustomerId
                }).Where(x => x.CustomerId == User.GetUserId().Value && x.BookTitle.Contains(search));
            else
                wishList = db.WishLists;

            var customerId = User.Identity.Name;
            /*if (customerId != null)
            {
                booksList = (from a in booksList
                             join b in db.Customers on a.BookID equals b.BookId
                             join c in db.User on b.CustomerId equals c.StripeCustomerId
                             where c.UserName == customerId
                             select a);
            }*/

            if (ViewBag.SortColumn == "BookTitle")
            {
                if (ViewBag.SortOrder == "up")
                {
                    wishList = wishList.OrderBy(temp => temp.BookTitle);
                }
                else
                {
                    wishList = wishList.OrderByDescending(temp => temp.BookTitle);
                }
            }

            int NoOfRecordsPerPage = 15;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(wishList.Count()) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = NoOfPages;

            return View(wishList.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList());
        }

        [HttpPost]
        public ActionResult DetailsWishList(int bookId) //placeholder for button handler
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            WishList wishList = new WishList
            {
                BookId = bookId,
                CustomerId = User.GetUserId().Value
            };

            db.WishLists.Add(wishList);
            db.SaveChanges();

            return RedirectToAction("UserProfile", "Account", new { id = bookId });
        }
    }
}