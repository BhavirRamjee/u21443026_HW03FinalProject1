using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using u21443026_HW03FinalProject1.Models;

namespace u21443026_HW03FinalProject1.Controllers
{
    public class TypeAuthorBorrowsController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: AuthorBorrowController

        public ActionResult TypeAuthorBorrows(int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.authors.OrderByDescending(x => x.authorId).ToList();
            IPagedList<authors> authors = list.ToPagedList(pageindex, pagesize);

            var list2 = db.types.OrderByDescending(x => x.typeId).ToList();
            IPagedList<types> type = list2.ToPagedList(pageindex, pagesize);


            var list3 = db.borrows.Include(b => b.books).Include(b => b.students).ToList();
            IPagedList<borrows> borrows = list3.ToPagedList(pageindex, pagesize);


            var studentbook = new
            {

                Types = type,
                Borrows = borrows,
                Authors = authors
            };

            ////////////////////////////////////////////////
            ///

            return View(studentbook);
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public async Task<ActionResult> AuthorIndex()
        {
            return View(await db.authors.ToListAsync());
        }

        public async Task<ActionResult> AuthorDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            authors authors = await db.authors.FindAsync(id);
            if (authors == null)
            {
                return HttpNotFound();
            }
            return View(authors);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorCreate([Bind(Include = "authorId,name,surname")] authors authors)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(authors);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(authors);
        }

        public async Task<ActionResult> AuthorEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            authors authors = await db.authors.FindAsync(id);
            if (authors == null)
            {
                return HttpNotFound();
            }
            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorEdit([Bind(Include = "authorId,name,surname")] authors authors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authors).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(authors);
        }

        public async Task<ActionResult> AuthortDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            authors authors = await db.authors.FindAsync(id);
            if (authors == null)
            {
                return HttpNotFound();
            }
            return View(authors);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorDeleteConfirmed(int id)
        {
            authors authors = await db.authors.FindAsync(id);
            db.authors.Remove(authors);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> BookIndex()
        {
            var books = db.books.Include(b => b.authors).Include(b => b.types);
            return View(await books.ToListAsync());
        }

        public async Task<ActionResult> BookDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        public ActionResult BookCreate()
        {
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookCreate([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        public async Task<ActionResult> BookEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookEdit([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        public async Task<ActionResult> BookDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookDeleteConfirmed(int id)
        {
            books books = await db.books.FindAsync(id);
            db.books.Remove(books);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}