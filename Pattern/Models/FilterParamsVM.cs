using System.ComponentModel.DataAnnotations;

namespace Pattern.Models
{
    public class FilterParamsVM
    {
        [Range(1, Int16.MaxValue, ErrorMessage = "invalid page index")]
        public int PageIndex { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "invalid page size")]
        public int PageSize { get; set; } = 10;

        [Display(Name = "Search ")]
        public string? ContainSearch { get; set; } = string.Empty;

        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2023, 1, 1);

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.UtcNow;

        [Range(0, 2, ErrorMessage = "Invalid status")]
        public int Status { get; set; } = 0;

        [Display(Name = "Sort By")]
        public string? SortCol { get; set; } = string.Empty;

        [Display(Name = "Sort Order")]
        public bool? SortDesc { get; set; }
    }
}
