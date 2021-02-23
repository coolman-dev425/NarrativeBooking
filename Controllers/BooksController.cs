using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApplication1.Identity;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using WebApplication1.Helper;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {
        public Book CurrentBook { get; set; }
        public UserManager<User> userManager { get; set; }
        public SignInManager<User> signInManager { get; set; }
        private const decimal AdminProfit = 10;
        private readonly IHostingEnvironment _environment;

        [DllImport("Maturity.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        private extern static int CalculateMaturity([MarshalAs(UnmanagedType.LPStr)]String text);

        private string GetTextFromDocx(MemoryStream mem)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, false))
            {
                Body body = wordDoc.MainDocumentPart.Document.Body;
                return body.InnerText;
            }
        }

        private BooksContext db;
        //private WishListContext _wishListContext;

        public BooksController(UserManager<User> userManager, SignInManager<User> signInManager, BooksContext context/*, WishListContext wishListContext*/,
            IHostingEnvironment environment)
        {
            db = context;
            //_wishListContext = wishListContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._environment = environment;
        }

        [HttpGet("List")]
        public IActionResult Get()
        {
            return Ok(db.Books.ToArray());

        }

        // GET: Books

        public IActionResult Index(string search = "", string SortColumn = "BookAuthor", string SortOrder = "p", string Genre1 = "", string Genre2 = "", int PageNo = 1, string SelectedGenre = "Contemporary")
        {
            ViewBag.SortColumn = SortColumn;
            ViewBag.SortOrder = SortOrder;
            ViewBag.search = search;
            ViewBag.Genre1 = Genre1;
            ViewBag.Genre2 = Genre2;
            IQueryable<Book> booksList;
            if (search != null)
                booksList = db.Books.Where(temp => temp.BookTitle.Contains(search) ||
                                                                    temp.BookAuthor.Contains(search) ||
                                                                    temp.Genre1.Contains(search) ||
                                                                    temp.Genre2.Contains(search) ||
                                                                    temp.Synopsis.Contains(search) ||
                                                                    temp.WritingType.Contains(search) ||
                                                                    temp.MaturityRating.Contains(search));
            else
                booksList = db.Books;

            //IQueryable<Book> booksListCustomer;
            var customerId = User.Identity.Name;
            /*if (customerId != null)
            {
                booksListCustomer = (from a in booksList
                             join b in db.Customers on a.BookID equals b.BookId
                             join c in db.User on b.CustomerId equals c.StripeCustomerId
                             where c.UserName == customerId
                             select a);
            }*/

            if (Genre1 != "0" && Genre1 != "")
            {
                booksList = booksList.Where(o => o.Genre1 == Genre1);
            }

            if (Genre2 != "0" && Genre2 != "")
            {
                booksList = booksList.Where(o => o.Genre2 == Genre2);
            }

            if (ViewBag.SortColumn == "BookTitle")
            {
                if (ViewBag.SortOrder == "up")
                {
                    booksList = booksList.OrderBy(temp => temp.BookTitle);
                }
                else
                {
                    booksList = booksList.OrderByDescending(temp => temp.BookTitle);
                }
            }

            if (ViewBag.SortColumn == "BookAuthor")
            {
                if (ViewBag.SortOrder == "up")
                {
                    booksList = booksList.OrderBy(temp => temp.BookAuthor);
                }
                else
                {
                    booksList = booksList.OrderByDescending(temp => temp.BookAuthor);
                }
            }

            if (ViewBag.SortColumn == "MaturityRating")
            {
                if (ViewBag.SortOrder == "up")
                {
                    booksList = booksList.OrderBy(temp => temp.MaturityRating);
                }
                else
                {
                    booksList = booksList.OrderByDescending(temp => temp.MaturityRating);
                }
            }

            if (ViewBag.SortColumn == "WritingType")
            {
                if (ViewBag.SortOrder == "up")
                {
                    booksList = booksList.OrderBy(temp => temp.WritingType);
                }
                else
                {
                    booksList = booksList.OrderByDescending(temp => temp.WritingType);
                }
            }

            int NoOfRecordsPerPage = 40;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(booksList.Count()) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = NoOfPages;

            List<Followers> followers = db.Followers.ToList();
            List<Book> book_list = booksList.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            ViewBag.totalCnt = book_list.Count;
            return View(booksList.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult LandingPage()
        {
            return View();
        }

        public ActionResult Disclaimer()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult Details(long id)
        {
            var userID = User.GetUserId();
            var b = db.Books.Where(temp => temp.BookId == id).FirstOrDefault();

            var userBook = new UserBookModel()
            {
                Book = b,
                IsInWishList = db.WishLists.Any(w => w.CustomerId == userID && w.BookId == b.BookId),
                UserId = userID,
                IsFollowed = db.Followers.Any(x => x.UserId == userID && x.FollowUserId == b.CreatedBy),
                IsPurchased = db.Purchased.Any(x=> x.CustomerId == userID && x.BookId == b.BookId)
            };

            if (userBook.Book.CoverPath == null || userBook.Book.CoverPath == "")
                ViewBag.CoverPath = "/images/default_book_cover.png";
            else
                ViewBag.CoverPath = "/images/" + userBook.Book.CoverPath;

            int followerCount = 0;
            followerCount = db.Followers.Where(w => w.FollowUserId == b.CreatedBy).Count();
            ViewBag.FollowCount = followerCount;
            return View(userBook);
        }

        /*[HttpPost]
        public ActionResult DetailsWishList(int BookID, string BookTitle) //placeholder for button handler
        {
            var user = db.User.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            var CustomerId = user.Id;
            WishList Addition = new WishList();
            Addition.BookId = BookID;
            Addition.BookTitle = BookTitle;
            Addition.CustomerId = CustomerId;
            var addedBook = db.WishList.Add(Addition); //in a controller this is other model
            db.SaveChanges(); //possibly move this to its own controller for its own model.

            //add CustomerID as current user's hash password.
            //linq add to database then make it a list in view see course 53
            // don't return View();
        }*/

        [HttpPost]
        public ActionResult DetailsPost(long bookId, decimal price)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("PaymentPage", new { id = bookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,BookTitle,BookFileName,BookAuthor,MaturityRating,Genre1,Genre2,WritingType,PageCount,Synopsis,WritingSample,AuthorBio,BookRowGuid,BookContent")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public ActionResult Edit(long id, [Bind("BookID,BookTitle,BookFileName,BookAuthor,MaturityRating,Genre1,Genre2,WritingType,PageCount,Synopsis,WritingSample,AuthorBio,BookRowGuid,BookContent")] Book book)
        {
            Book currentBook = db.Books.Where(temp => temp.BookId == id).FirstOrDefault();
            return View(currentBook);
        }

        [HttpPost]
        public ActionResult Edit(Book b)
        {
            Book currentBook = db.Books.Where(temp => temp.BookId == b.BookId).FirstOrDefault();
            currentBook.BookTitle = b.BookTitle;
            currentBook.BookAuthor = b.BookAuthor;
            currentBook.Genre1 = b.Genre1;
            currentBook.Genre2 = b.Genre2;
            currentBook.WritingType = b.WritingType;
            currentBook.PageCount = b.PageCount;
            currentBook.Synopsis = b.Synopsis;
            currentBook.WritingSample = b.WritingSample;
            currentBook.AuthorBio = b.AuthorBio;
            db.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        public ActionResult Delete(long id, [Bind("BookID,BookTitle,BookFileName,BookAuthor,MaturityRating,Genre1,Genre2,WritingType,PageCount,Synopsis,WritingSample,AuthorBio,BookRowGuid,BookContent")] Book book)
        {
            Book currentBook = db.Books.Where(temp => temp.BookId == id).FirstOrDefault();
            return View(currentBook);
        }

        [HttpPost]
        public ActionResult Delete(Book b)
        {
            Book currentBook = db.Books.Where(temp => temp.BookId == b.BookId).FirstOrDefault();
            db.Books.Remove(currentBook);
            db.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        private bool BookExists(long id)
        {
            return db.Books.Any(e => e.BookId == id);
        }

        private String GetRatingDescription(int value)
        {
            if (value >= 0 && value <= 10)
            {
                return String.Format("Everyone", value);
            }
            else if (value >= 11 && value <= 25)
            {
                return String.Format("Discerning", value);
            }
            else if (value >= 26 && value <= 155)
            {
                return String.Format("Adult", value);
            }
            else if (value >= 156 && value <= 220)
            {
                return String.Format("Disruptive", value);
            }
            else
            {
                return String.Format("Error not integer {0}", value);
            }
        }

        public ActionResult UploadForm()
        {
            var customerId = "";
            if (User.Identity.IsAuthenticated)
            {
                var currUser = db.User.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
                //customerId = new Random().ToString();
                customerId = currUser.PasswordHash;
            }

            /*var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1000,
                Currency = "aud",
            };

            if (customerId != "")
                options.CustomerId = customerId;

            var intent = service.Create(options);
            ViewData["PaymentIntentId"] = intent.Id;
            ViewData["ClientSecret"] = intent.ClientSecret;*/

            return View();
        }

        [HttpPost]
        public ActionResult UploadForm(UploadDTO model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                string text = "";
                byte[] content = null;
                string maturity = "Not calculated";

                using (MemoryStream ms = new MemoryStream())
                {
                    model.docFile.CopyTo(ms);
                    content = ms.ToArray();
                    text = GetTextFromDocx(ms);
                    maturity = GetRatingDescription(CalculateMaturity(text));
                }

                HttpContext.Session.SetString("BookTitle", model.book_title);
                HttpContext.Session.SetString("BookFileName", model.docFile.FileName);
                HttpContext.Session.SetString("Price", decimal.Parse(model.book_price).ToString());
                HttpContext.Session.Set("BookContent", content);
                HttpContext.Session.SetString("MaturityRating", maturity);
                HttpContext.Session.SetString("Genre1", model.Genre1);
                HttpContext.Session.SetString("Genre2", model.Genre2);
                HttpContext.Session.SetString("WritingType", model.WritingType);
                HttpContext.Session.SetString("Synopsis", model.Synopsis);
                HttpContext.Session.SetString("PageCount", model.PageCount.ToString());
                HttpContext.Session.SetString("BookAuthor", model.BookAuthor);
                HttpContext.Session.SetString("AuthorBio", model.AuthorBio);
                HttpContext.Session.SetString("WritingSample", model.WritingSample);
                HttpContext.Session.SetString("Editing", model.Editing);
                HttpContext.Session.SetString("CreatedBy", "0");

                return RedirectToAction("Login", "Account", new { status = "1" });
            } else {
                int? userId = User.GetUserId();
                User currentUser = db.User.Where(m => m.Id == userId).FirstOrDefault();

                if (currentUser.Status == 1)
                {
                    String result = "Not started";
                    try
                    {
                        string text = "";
                        byte[] content = null;
                        string maturity = "Not calculated";

                        var customerId = currentUser.PasswordHash;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            model.docFile.CopyTo(ms);
                            content = ms.ToArray();
                            text = GetTextFromDocx(ms);
                            maturity = GetRatingDescription(CalculateMaturity(text));
                        }

                        Book N = new Book();
                        N.BookTitle = model.book_title;
                        N.BookFileName = model.docFile.FileName;
                        N.Price = decimal.Parse(model.book_price);
                        N.BookContent = content;
                        N.MaturityRating = maturity;
                        N.Genre1 = model.Genre1;
                        N.Genre2 = model.Genre2;
                        N.WritingType = model.WritingType;
                        N.Synopsis = model.Synopsis;
                        N.PageCount = model.PageCount;
                        N.BookAuthor = model.BookAuthor;
                        N.AuthorBio = model.AuthorBio;
                        N.WritingSample = model.WritingSample;
                        N.Editing = model.Editing;
                        if (HttpContext.Session.GetString("CoverPath").ToString() == null || HttpContext.Session.GetString("CoverPath").ToString() == "")
                            N.CoverPath = "";
                        N.CoverPath = HttpContext.Session.GetString("CoverPath").ToString();
                        N.CreatedBy = currentUser.Id;

                        var bookResult = db.Books.Add(N);
                        db.SaveChanges();

                        // Create a Customer:
                        var newCustomer = new CustomerBooks
                        {
                            BookId = bookResult.Entity.BookId,
                            CustomerId = customerId,
                            IsStripeUser = true
                        };

                        db.Add<CustomerBooks>(newCustomer);

                        var saveResult = db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }

                    return RedirectToAction("UserProfile", "Account");
                } else {
                    string text = "";
                    byte[] content = null;
                    string maturity = "Not calculated";

                    using (MemoryStream ms = new MemoryStream())
                    {
                        model.docFile.CopyTo(ms);
                        content = ms.ToArray();
                        text = GetTextFromDocx(ms);
                        maturity = GetRatingDescription(CalculateMaturity(text));
                    }

                    HttpContext.Session.SetString("BookTitle", model.book_title);
                    HttpContext.Session.SetString("BookFileName", model.docFile.FileName);
                    HttpContext.Session.SetString("Price", decimal.Parse(model.book_price).ToString());
                    HttpContext.Session.Set("BookContent", content);
                    HttpContext.Session.SetString("MaturityRating", maturity);
                    HttpContext.Session.SetString("Genre1", model.Genre1);
                    HttpContext.Session.SetString("Genre2", model.Genre2);
                    HttpContext.Session.SetString("WritingType", model.WritingType);
                    HttpContext.Session.SetString("Synopsis", model.Synopsis);
                    HttpContext.Session.SetString("PageCount", model.PageCount.ToString());
                    HttpContext.Session.SetString("BookAuthor", model.BookAuthor);
                    HttpContext.Session.SetString("AuthorBio", model.AuthorBio);
                    HttpContext.Session.SetString("WritingSample", model.WritingSample);
                    HttpContext.Session.SetString("Editing", model.Editing);
                    HttpContext.Session.SetString("CreatedBy", "0");

                    return RedirectToAction("MemberShip", "Account");
                }
            }
        }

        [HttpPost]
        public ActionResult UploadCover(FileDataModel model)
        {
            if (model.File == null)
            {
                HttpContext.Session.SetString("CoverPath", "");
                return Json(new
                {
                    message = "Fail",
                    data = "",
                    error = true
                });
            }
                
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    message = "Something went wrong.",
                    error = true
                });
            }

            var fileName = Path.GetFileName(model.File.FileName);
            var newFileName = "bookcover_" + fileName;
            if (newFileName == null || newFileName == "")
                HttpContext.Session.SetString("CoverPath", "");
            else
                HttpContext.Session.SetString("CoverPath", newFileName);
            fileName = Path.Combine(_environment.WebRootPath, "images") + $@"\{newFileName}";

            using (var fs = System.IO.File.Create(fileName))
            {
                model.File.CopyTo(fs);
                fs.Flush();
            }

            return Json(new
            {
                message = "Success",
                data = fileName,
                error = false
            });
        }

        public ActionResult PaymentPage(long id)
        {
            Book b = db.Books.Where(temp => temp.BookId == id).FirstOrDefault();
            var price = (b.Price.Value * 100).ToString().Split('.');

            //var service = new PaymentIntentService();
            //var options = new PaymentIntentCreateOptions
            /*{
                Amount = long.Parse(price[0]),
                Currency = "aud"
            };*/

            //TODO: Uncomment this block of code before release
            //var intent = service.Create(options);
            //ViewData["PaymentIntentId"] = intent.Id;
            //ViewData["ClientSecret"] = intent.ClientSecret;
            //ViewData["BookId"] = id;
            //ViewData["Price"] = b.Price.Value;
            return RedirectToAction("PaymentPageExecute", "Books", new { bookId = id, paymentIntentId = 0 });
        }

        //[HttpPost]
        public ActionResult PaymentPageExecute(long bookId, string paymentIntentId)
        {
            //if (transfer.Id != null)
            if (true)
            {
                //Book currentBook = db.Books.Where(temp => temp.BookId == bookId).FirstOrDefault();
                //return File(currentBook.BookContent, "APPLICATION/octet-stream", currentBook.BookFileName);
                var purchased = new Purchased()
                {
                    BookId = bookId,
                    CustomerId = User.GetUserId().Value
                };

                db.Purchased.Add(purchased);
                db.SaveChanges();
                return RedirectToAction("UserProfile", "Account");

            }
            else
            {
                return RedirectToAction("Index", "Books");
            }
        }

        [HttpPost]
        public ActionResult RemoveWishlist(int bookId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            WishList wishList = db.WishLists.FirstOrDefault(x => x.CustomerId == User.GetUserId().Value && x.BookId == bookId);
            db.WishLists.Remove(wishList);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = bookId });
        }
    }
}
