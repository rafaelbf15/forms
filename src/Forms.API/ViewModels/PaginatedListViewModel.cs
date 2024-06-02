using System.Collections.Generic;

namespace Forms.API.Models
{
    public class PaginatedListViewModel<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public int PreviousPageIndex { get; set; }
        public bool HasNextPage { get; set; }
        public int NextPageIndex { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
