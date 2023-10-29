using PagedList;
using System.Collections.Generic;

namespace u21443026_HW03FinalProject1.Models
{
    public class StudentBooksVM
    {
        public IEnumerable<students> students { get; set; }
        public IPagedList<books> Books { get; set; }
        public IPagedList<borrows> Borrows { get; set; }
        public IPagedList<types> Types { get; set; }
        public IPagedList<authors> Authors { get; set; }
        public IPagedList<students> PageStudents { get; set; }
    }
}