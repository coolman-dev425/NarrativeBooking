using Mammoth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Helper;
using WebApplication1.Identity;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using WebApplication1.Models.Identify;
using WebApplication1.Models.Identity;

namespace WebApplication1.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signIn;
        private readonly UserManager<User> userManager;
        private readonly IHostingEnvironment _environment;
        private readonly BooksContext db;
        //private WishListContext _wishListContext;

        public AccountController(BooksContext context,
            /*WishListContext wishListContext, */
            SignInManager<User> signIn,
            UserManager<User> userManager,
            IHostingEnvironment environment)
        {
            db = context;
            //_wishListContext = wishListContext;
            this.signIn = signIn;
            this.userManager = userManager;
            this._environment = environment;
        }

        public ActionResult Login(string status = "")
        {
            if (status == "")
                ViewBag.Status = "0";
            else
                ViewBag.Status = "1";

            LoginDTO loginModel = new LoginDTO();
            return View(loginModel);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            User currentUser = db.User.Where(temp => temp.UserName == model.UserName).FirstOrDefault();
            
            if (currentUser == null) {
                ViewBag.Message = "User is not here!";
                return View(model);
            }

            if (currentUser.Status == 0)
            {
                var result = await signIn.PasswordSignInAsync(model.UserName, model.Password, model.IsRemembered, true);
                if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var userExists = await userManager.FindByNameAsync(currentUser.UserName);
                    return RedirectToAction("UserProfile", "Account");
                } else
                {
                    ViewBag.Message = "User doesn't exist or wrong password";
                    return View(model);
                }
            }
            else
            {
                if (model.Password != null)
                {
                    try
                    {
                        var result = await signIn.PasswordSignInAsync(model.UserName, model.Password, model.IsRemembered, true);
                        if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                        {
                            var userExists = await userManager.FindByNameAsync(model.UserName);
                            return RedirectToAction("UserProfile", "Account");
                        }
                        else
                        {
                            ViewBag.Message = "User doesn't exist or wrong password";
                            return View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Message = "Password and Confirm Password different";
                    return View(model);
                }
            }
        }

        public ActionResult ForgotPassword()
        {
            ForgotPasswordDTO forgotPasswordModel = new ForgotPasswordDTO();
            return View(forgotPasswordModel);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordDTO model)
        {
            var Password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var Email = model.Email;
            User currentUser = db.User.Where(temp => temp.Email == Email).FirstOrDefault();

            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.Message = "You should be login";
                return View(model);
            }

            if (currentUser == null)
            {
                ViewBag.Message = "User doesn't exists";
                return View(model);
            }

            if (!(Password == confirmPassword))
            {
                ViewBag.Message = "New Password and ConfirmPassword must be the same!";
                return View(model);
            } else
            {
                if (userManager.IsEmailConfirmedAsync(currentUser).Result)
                {
                    ViewBag.Message = "Error while resetting your password!";
                    return View("Error");
                }

                var token = userManager.GeneratePasswordResetTokenAsync(currentUser).Result;
                IdentityResult result = userManager.ResetPasswordAsync(currentUser, token, Password).Result;
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Message = "Error while resetting the password!";
                    return View(model);
                }
            }
        }

        public ActionResult MemberShip()
        {
            MemberShipDTO membershipModel = new MemberShipDTO();
            return View(membershipModel);
        }

        [HttpPost]
        public async Task<ActionResult> MemberShip(MemberShipDTO model)
        {
            var UserName = model.UserName;
            var Email = model.Email;
            var Password = model.Password;
            var confirmPassword = model.ConfirmPassword;

            User currentUser = db.User.Where(temp => temp.UserName == UserName).Where(temp => temp.Email == Email).FirstOrDefault();
            User loginUser = db.User.Where(temp => temp.UserName == User.Identity.Name).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                if (currentUser == null)
                {
                    ViewBag.Message = "Input Information Correctly!";
                    return View(model);
                } else if (currentUser.Email != loginUser.Email)
                {
                    ViewBag.Message = "Input Information Correctly! You are not logined User!";
                    return View(model);
                }
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Message = "Password and ConfirmPassword Must be Same!";
                return View(model);
            }

            Book N = new Book();

            N.BookTitle = HttpContext.Session.GetString("BookTitle");
            N.BookFileName = HttpContext.Session.GetString("BookFileName");
            N.Price = Convert.ToDecimal(HttpContext.Session.GetString("Price"));
            N.BookContent = HttpContext.Session.Get("BookContent");
            N.MaturityRating = HttpContext.Session.GetString("MaturityRating");
            N.Genre1 = HttpContext.Session.GetString("Genre1").ToString();
            N.Genre2 = HttpContext.Session.GetString("Genre2").ToString();
            N.WritingType = HttpContext.Session.GetString("WritingType").ToString();
            N.Synopsis = HttpContext.Session.GetString("Synopsis").ToString();
            N.PageCount = System.Convert.ToInt32(HttpContext.Session.GetString("PageCount"));
            N.BookAuthor = HttpContext.Session.GetString("BookAuthor").ToString();
            N.AuthorBio = HttpContext.Session.GetString("AuthorBio").ToString();
            N.WritingSample = HttpContext.Session.GetString("WritingSample").ToString();
            N.Editing = HttpContext.Session.GetString("Editing").ToString();
            if (HttpContext.Session.GetString("CoverPath").ToString() == null || HttpContext.Session.GetString("CoverPath").ToString() == "")
                N.CoverPath = "";
            else
                N.CoverPath = HttpContext.Session.GetString("CoverPath").ToString();

            if (currentUser == null)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    FullName = model.UserName,
                    Email = model.Email,
                    FollowType = FollowType.Follower,
                    Status = 1
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result == IdentityResult.Success)
                {
                    var resultSignIn = await signIn.PasswordSignInAsync(user, model.Password, true, true);
                    if (resultSignIn == Microsoft.AspNetCore.Identity.SignInResult.Success)
                    {
                        var userExists = await userManager.FindByNameAsync(model.UserName);

                        User created_user = db.User.Where(temp => temp.Email == Email).FirstOrDefault();
                        try {
                            N.CreatedBy = created_user.Id;
                            var bookResult = db.Books.Add(N);
                            db.SaveChanges();

                            // Create a Customer:
                            var newCustomer = new CustomerBooks
                            {
                                BookId = bookResult.Entity.BookId,
                                CustomerId = created_user.PasswordHash,
                                IsStripeUser = true
                            };

                            db.Add<CustomerBooks>(newCustomer);
                            var saveResult = db.SaveChanges();

                        } catch (Exception ex) {
                            
                        }

                        return RedirectToAction("UserProfile", "Account");
                    }
                    else
                    {
                        model.Password = "";
                        model.ConfirmPassword = "";
                        ViewBag.Message = "User doesn't exists or wrong password";
                        return View(model);
                    }
                }
                else {
                    model.Password = "";
                    model.ConfirmPassword = "";
                    var message = "";
                    foreach (var error in result.Errors)
                        message += error.Description + ", ";
                    ViewBag.Message = message.Remove(message.Length - 2, 2);
                    return View(model);
                }
            } else {
                if (currentUser.Status == 0) {
                    try {

                        currentUser.Status = 1;
                        db.SaveChanges();
                        
                        N.CreatedBy = currentUser.Id;
                        var bookResult = db.Books.Add(N);
                        db.SaveChanges();

                        // Create a Customer:
                        var newCustomer = new CustomerBooks
                        {
                            BookId = bookResult.Entity.BookId,
                            CustomerId = currentUser.PasswordHash,
                            IsStripeUser = true
                        };

                        db.Add<CustomerBooks>(newCustomer);
                        var saveResult = db.SaveChanges();
                    } catch (Exception ex) {
                    }
                } else {
                    ViewBag.Message = "You are already member!";
                    return View(model);
                }
            }

            return RedirectToAction("UserProfile", "Account");
        }

        public ActionResult Contact()
        {
            ContactDTO contactModel = new ContactDTO();
            return View(contactModel);
        }

        [HttpPost]
        public ActionResult Contact(ContactDTO model)
        {
            var Email = model.Email;
            var Content = model.Content;

            Contact contact = new Contact();

            contact.Email = Email;
            contact.Content = Content;

            User currentUser = db.User.Where(m => m.Email == model.Email).FirstOrDefault();
            if (currentUser == null)
            {
                ViewBag.Message = "You are not registered yet.";
                return View(model);
            } else
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                ViewBag.Message = "Success.";
                return View(model);
            }
        }
        
        public ActionResult Register()
        {
            RegisterDTO model = new RegisterDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
           
            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.UserName,
                Email = model.Email,
                FollowType = FollowType.Follower,
                Status = 0
            };

            User currUser = db.User.Where(m => m.UserName == model.UserName).Where(m => m.Email == model.Email).FirstOrDefault();
            
            if (currUser == null)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result == IdentityResult.Success)
                    {
                        var resultSignIn = await signIn.PasswordSignInAsync(user, model.Password, true, true);
                        if (resultSignIn == Microsoft.AspNetCore.Identity.SignInResult.Success)
                        {
                            var userExists = await userManager.FindByNameAsync(model.UserName);
                            return RedirectToAction("UploadForm", "Books");
                        }
                        else
                        {
                            model.Password = "";
                            model.ConfirmPassword = "";
                            ViewBag.Message = "User doesn't exists or wrong password";
                            return View(model);
                        }
                    }
                    else
                    {
                        model.Password = "";
                        model.ConfirmPassword = "";
                        var message = "";
                        foreach (var error in result.Errors)
                            message += error.Description + ", ";
                        ViewBag.Message = message.Remove(message.Length - 2, 2);
                        return View(model);
                    }
                } else {
                    ViewBag.Message = "Password and ConfirmPassword Must be Same!";
                    return View(model);
                }
            } else
            {
                ViewBag.Message = "Already registered";
                return View(model);
            }
        }

        public async Task<ActionResult> Logout()
        {
            await signIn.SignOutAsync();
            return RedirectToAction("Index", "Books");
        }
        public ActionResult UserProfile()
        {
            int? userId = User.GetUserId();
            var wishList = db.WishLists.Where(w => w.CustomerId == userId.Value)
                .Join(db.Books, w => w.BookId, b => b.BookId, (w, b) => new WishList
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    CustomerId = w.CustomerId
                }).ToList();
            var purchased = db.Purchased.Where(p => p.CustomerId == userId.Value)
                .Join(db.Books, p => p.BookId, b => b.BookId, (p, b) => new Purchased
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    CustomerId = p.CustomerId
                });

            var books = db.Books;

            return View(new UserProfileModel()
            {
                Purchased = purchased,
                WishList = wishList,
                Books = books,
                HasAvatar = System.IO.File.Exists($@"{_environment.WebRootPath}/images/useravatar_{User.Identity.Name}.png")
            });
        }

        [HttpPost]
        public async Task<ActionResult> UserProfile(string name)
        {
            //code for delete account, use same code getting books to list them in profile:
            var user = db.User.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            var custBooks = db.Customers.Where(m => m.CustomerId == user.StripeCustomerId).ToList();
            foreach (var book in custBooks)
            {                
                db.Customers.Remove(book);
            }

            var currBooks = db.Books.Where(m => m.CreatedBy == user.Id);
            db.Books.RemoveRange(currBooks);

            var followes = db.Followers.Where(x => x.FollowUserId == user.Id || x.UserId == user.Id);
            db.Followers.RemoveRange(followes);

            await userManager.DeleteAsync(user);
            await signIn.SignOutAsync();
            return RedirectToAction("Index", "Books");
        }

        [HttpPost]
        public async Task<ActionResult> AddFollower(long bookId)
        {
            Book book = db.Books.Where(x => x.BookId == bookId).FirstOrDefault();
            if (User.GetUserId() != null && book != null && book.CreatedBy.HasValue)
            {
                Followers followers = new Followers
                {
                    UserId = User.GetUserId().Value,
                    FollowUserId = book.CreatedBy.Value
                };
                db.Followers.Add(followers);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Books", new { id = bookId });
        }


        [HttpPost]
        public async Task<ActionResult> Unfollow(long bookId)
        {
            Book book = db.Books.Where(x => x.BookId == bookId).FirstOrDefault();
            if (User.GetUserId() != null && book != null)
            {
                Followers followers = db.Followers.Where(x => x.UserId == User.GetUserId() && x.FollowUserId == book.CreatedBy).FirstOrDefault();
                db.Followers.Remove(followers);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        public async Task<ActionResult> UserChat()
        {
            int? userId = User.GetUserId();
            if (userId.HasValue)
            {
                IEnumerable<Followers> followers = db.Followers.Where(x => x.UserId == userId.Value || x.FollowUserId == userId.Value).ToList();
                IEnumerable<User> followUsers = db.User.Where(x => followers.Any(y => (y.UserId == x.Id || y.FollowUserId == x.Id)) && x.Id != userId.Value).ToList()
                    .Select(x =>
                    {
                        x.IsOnline = CacheHelper.Instance.IsOnline(x.UserName);
                        x.FollowType = GetFollowType(x.Id, userId.Value, followers);
                        x.HasAvatar = System.IO.File.Exists($"{_environment.WebRootPath}/images/useravatar_{x.UserName}.png");
                        return x;
                    });

                UserChatModel ret = new UserChatModel
                {
                    HasAvatar = System.IO.File.Exists($"{_environment.WebRootPath}/images/useravatar_{User.Identity.Name}.png"),
                    FollowUsers = followUsers
                };

                return View(ret);
            }
            return View();
        }

        [HttpPost]
        public ActionResult RemoveWishlist(int bookId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            WishList wishList = db.WishLists.FirstOrDefault(x => x.CustomerId == User.GetUserId().Value && x.BookId == bookId);
            db.WishLists.Remove(wishList);
            db.SaveChanges();

            return RedirectToAction("UserProfile");
        }

        public ActionResult ReadBook(long bookId)
        {
            var styleMap = "p[style-name='Heading 1'] => h1\n" +
                "p[style-name='Title'] => h1.section-title:fresh\n" +
                "p[style-name='Subsection Title'] => h2\n" +
                "b => b\n" +
                "i => strong\n" +
                "u => em\n" +
                "strike => del\n" +
                "comment-reference => sup";

            var author_id = db.Books.FirstOrDefault(b => b.BookId == bookId).CreatedBy;
            if (author_id == null)
                return RedirectToAction("Login", "Account");

            var author_status = db.User.FirstOrDefault(u => u.Id == author_id).Status;

            var bookContent = db.Books.FirstOrDefault(b => b.BookId == bookId).BookContent;
            try
            {
                var stream = new MemoryStream(bookContent);
                var convertion = new DocumentConverter()
                    .AddStyleMap(styleMap);
                var result = convertion.ConvertToHtml(stream);
                ViewBag.bookId = bookId;
                return View((object)result.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult RatingBook(RatingDTO model)
        {
            int rating_val = model.ratingvalue;
            long book_id = model.bookid;

            Book currentBook = db.Books.Where(temp => temp.BookId == book_id).FirstOrDefault();

            if (currentBook == null) {
                ViewBag.Message = "Book doesn't exists";
                return View(model);
            } else {
                currentBook.RatingTimes += 1;
                currentBook.RatingVal += rating_val;
                currentBook.RatingNum = Convert.ToInt32(currentBook.RatingVal / currentBook.RatingTimes);
                db.SaveChanges();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UploadAvatar(FileDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    message = "Something went wrong.",
                    error = true
                });
            }

            var username = User.Identity.Name;
            var fileName = Path.GetFileName(model.File.FileName);
            var newFileName = "useravatar_" + username + ".png";
            fileName = Path.Combine(_environment.WebRootPath, "images") + $@"\{newFileName}";

            using (var fs = System.IO.File.Create(fileName))
            {
                model.File.CopyTo(fs);
                fs.Flush();
            }

            return Json(new
            {
                message = $"Change avatar successfully.",
                data = fileName,
                error = false
            });
        }

        private FollowType GetFollowType(int followUserId, int loggedInUserId, IEnumerable<Followers> followers)
        {
            if (followers.Count(x => (x.UserId == followUserId && x.FollowUserId == loggedInUserId) || (x.UserId == loggedInUserId && x.FollowUserId == followUserId)) >= 2)
                return FollowType.MutualFollow;
            else if (followers.Any(x => x.UserId == followUserId && x.FollowUserId == loggedInUserId))
                return FollowType.Follower;

            return FollowType.Following;

        }
    }
}