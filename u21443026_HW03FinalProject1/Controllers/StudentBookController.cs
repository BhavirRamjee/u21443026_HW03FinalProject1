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
    public class StudentBookController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: StudentBook

        public async Task<ActionResult> StudentBook(int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.students.OrderByDescending(x => x.studentId).ToList();
            IPagedList<students> students = list.ToPagedList(pageindex, pagesize);

            var booksList = db.books.Include(b => b.authors).Include(b => b.types).ToList();
            IPagedList<books> books = booksList.ToPagedList(pageindex, pagesize);


            var studentbook = new StudentBooksVM
            {

                students = await db.students.ToListAsync(),
                Books = books,
                PageStudents = students
            };
            return View(studentbook);
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> StudentIndex()
        {
            return View(await db.students.ToListAsync());
        }

        public async Task<ActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> StudentCreate([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(students);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(students);
        }

        public async Task<ActionResult> StudentEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentEdit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(students);
        }

        public async Task<ActionResult> StudentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentDeleteConfirmed(int id)
        {
            students students = await db.students.FindAsync(id);
            db.students.Remove(students);
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