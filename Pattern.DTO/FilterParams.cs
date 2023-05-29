namespace Pattern.DTO
{
    public class FilterParams
    {
        public FilterParams(int pageIndex, int pageSize, string containSearch, DateTime startDate, DateTime endDate, string sortCol="name", bool sortDesc=false, int status=0)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            ContainSearch = containSearch;
            StartDate = startDate;
            EndDate = endDate;
            SortCol = sortCol;
            Status = status;
            SortDesc = sortDesc;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ContainSearch { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string SortCol { get; set; } = string.Empty;
        public bool SortDesc { get; set; }
    }
}
