using System.ComponentModel.DataAnnotations;

namespace Pattern.Models
{
    public class PageListVM<T> where T : class
    {
        public PageListVM(int pageIndex, int pageSize, int totalRecords, List<T> records)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Records = records;
            TotalRecords = totalRecords;
        }

        [Required(ErrorMessage="Page index required")]
        [Range(1,Int16.MaxValue,ErrorMessage ="invalid page index")]
        public int PageIndex { get; set; }

        [Required(ErrorMessage = "Page size required")]
        [Range(1, Int16.MaxValue, ErrorMessage = "invalid page size")]
        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
        public List<T> Records { get; set; } = new List<T>();
    }
}
