using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Public.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Default/

        public void LogIn(string username, string password)
        {
            string SQL="SELECT * FROM BORROWER WHERE UserName='"+username+"'";
            string g = "";
        }
        //
        // GET: /Authors/

        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                Library_BL.User user = (Library_BL.User)Session["User"];
                ViewBag.userName = user.UserName;
                ViewBag.showLoginPanel = false;
            }
            else
            {
                ViewBag.showLoginPanel = true;
            }
            return View();
        }

        public ActionResult BorrowerBorrower()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult AdminAuthor()
        {
            return View();
        }
        public ActionResult IndexAdmin()
        {
            if (Session["User"] != null)
            {
                Library_BL.User user = (Library_BL.User)Session["User"];
                ViewBag.userName = user.UserName;
                ViewBag.showLoginPanel = false;
            }
            else
            {
                ViewBag.showLoginPanel = true;
            }
            return View();
        }
        public ActionResult IndexUser()
        {
            if (Session["User"] != null)
            {
                Library_BL.User user = (Library_BL.User)Session["User"];
                ViewBag.userName = user.UserName;
                ViewBag.showLoginPanel = false;
            }
            else
            {
                ViewBag.showLoginPanel = true;
            }
            return View();
        }

        public ActionResult AdminBorrower()
        {
            return View();
        }
        public ActionResult AdminBook()
        {
            return View();
        }
        public ActionResult PublicSearchPage()
        {
            return View();
        }
        public ActionResult PublicBrowsePage()
        {
            return View();
        }
        public ActionResult PublicBookDetail()
        {
            return View();
        }

        public ActionResult ListAuthors()
        {
            return View(Library_BL.Author.getAll());        
        }

        public ActionResult createAuthor()
        {
            return View();
        }
        [HttpPost]

        public ActionResult createAuthor(FormCollection collection)
        {
            try
            {
                Library_BL.Author author = new Library_BL.Author();
                author.BirthYear = Convert.ToInt16(collection["BirthYear"]);
                author.FirstName = collection["FirstName"];
                author.LastName = collection["LastName"];
                author.save();
                return RedirectToAction("ListAuthors");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult editAuthor(int id)
        {
            return View(Library_BL.Author.getByAid(id));
        }
        [HttpPost]

        public ActionResult editAuthor(int id,FormCollection collection)
        {
            try
            {
                Library_BL.Author author = Library_BL.Author.getByAid(id);
                author.BirthYear = Convert.ToInt16(collection["BirthYear"]);
                author.FirstName = collection["FirstName"];
                author.LastName = collection["LastName"];
                author.save();
                return RedirectToAction("ListAuthors");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteAuthor(int id)
        {

                Library_BL.Author.DeleteAuthor(id);
                return RedirectToAction("ListAuthors");
        }
        public ActionResult Details(int id)
        {
            List<Library_BL.Book> books = Library_BL.Book.getByAuthor(id);
            ViewBag.Author = Library_BL.Author.getByAid(id);
            return View(books);
        }
        public ActionResult browserBookDetails(int id)
        {
            List<Library_BL.Book> books = Library_BL.Book.getByAuthor(id);
            ViewBag.Author = Library_BL.Author.getByAid(id);
            return View(books);
        }

        public ActionResult searchAuthors()
        {
            string str = Request.QueryString.Get("searchA");
            return View("ListAuthors", Library_BL.Author.search(str));
        }

        public ActionResult searchUsers()
        {
            string str = Request.QueryString.Get("searchU");
            return View("ListUsers", Library_BL.User.search(str));
        }

        public ActionResult usearchBooks()
        {
            string str = Request.QueryString.Get("usearchB");
            return View("userListBooks", Library_BL.Book.search(str));
        }

        public ActionResult searchBooks()
        {
            string str = Request.QueryString.Get("searchB");
            return View("ListBooks", Library_BL.Book.search(str));
        }

        public ActionResult usearchAuthors()
        {
            string str = Request.QueryString.Get("usearchA");
            return View("userListAuthors", Library_BL.Author.search(str));
        }

        public ActionResult ListBooks()
        {
            return View(Library_BL.Book.getAll());
        }

        public ActionResult userListAuthors()
        {
            return View(Library_BL.Author.getAll());
        }

        public ActionResult userListBooks()
        {
            return View(Library_BL.Book.getAll());
        }
        
        public ActionResult ListUsers()
        {
            return View(Library_BL.User.getAll());
        }


        //
        // GET: /User/Details/5

        public ActionResult DetailsUser(int id)
        {
            return View();
        }
        public ActionResult createUser()
        {
            return View();
        }

        //
        // GET: /User/Delete/5

        public ActionResult DeleteUser(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        public ActionResult DeleteUser(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateUser(FormCollection collection)
        {
            try
            {
                Library_BL.User user = new Library_BL.User();
                user.FirstName = collection["FirstName"];
                user.LastName = collection["LastName"];
                user.PersonId = collection["PersonId"];
                user.UserName = collection["UserName"];
                user.Password = collection["Password"];
                user.save();
                return RedirectToAction("ListUsers");
         
            }
            catch
            {
                return View();
            }
        }
        public ActionResult editUser(string personid)
        {
            return View(Library_BL.User.getByPersonId(personid));
        }
        [HttpPost]

        public ActionResult editUser(string personid, FormCollection collection)
        {
            try
            {
                Library_BL.User user = Library_BL.User.getByPersonId(personid);
                user.PersonId = collection["PersonId"];
                user.FirstName = collection["FirstName"];
                user.LastName = collection["LastName"];
                user.UserName = collection["username"];
                user.Password = collection["password"];
                user.save();
                return RedirectToAction("ListUsers");
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public ActionResult borrowBook (string isbn)
        {
            if (Session["User"] != null)
            {
                Library_BL.User user = (Library_BL.User)Session["User"];
                Library_BL.User.borrowBook(user.PersonId, isbn);
                return RedirectToAction("userListBooks");
            }
            else
            {
                return RedirectToAction("userListBooks");
            }
        }
        public ActionResult BorrowedBooks()
        {
             Library_BL.User user = (Library_BL.User)Session["User"];
             return View(Library_BL.Borrow.getBorrowedBooks(user.PersonId));
        }
        public ActionResult Reborrow(int barcode)
        {
            Library_BL.Borrow.Reborrow(barcode);
            return RedirectToAction("BorrowedBooks");
        }

        public ActionResult createBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult createBook(int id, FormCollection collection)
        {
            try
            {
                Library_BL.Book book = new Library_BL.Book();
                book.Title = collection["Title"];
                book.StatusId = 1;
                book.ISBN = collection["ISBN"];
                book.Aid = id;
                book.save();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ListAuthors");
        }

        public ActionResult editBook(string isbn, string title)
        {
            Library_BL.Book book = new Library_BL.Book();
            book = book.getByISBN(isbn,title);
            return View(book);
        }

        [HttpPost]

        public ActionResult editBook(string isbn, string title,FormCollection collection)
        {
            try
            {
                
                Library_BL.Book book = new Library_BL.Book();
                book.getByISBN(isbn,title);
                book.Title = collection["Title"];
                book.ISBN = collection["ISBN"];
                book.OLDISBN = collection["OLDISBN"];
                book.update();
                return RedirectToAction("ListBooks");
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public ActionResult DeleteBook(string isbn)
        {
            Library_BL.Book.delete(isbn);
            return RedirectToAction("ListBooks");
        }



        public ActionResult BookDetail(string isbn)
        {
            List<Library_BL.Author> author = new List<Library_BL.Author>();
            author = Library_BL.Author.getBookDetails(isbn);
            int coun = author.Count();
            for (int i = 0; i < coun; i++)
            {
                author[i] = Library_BL.Author.getByAid(author[i].Aid);
                
            }
            return View(author);
        }
        public ActionResult Widget()
        {
            return View();
        }

    }
}
