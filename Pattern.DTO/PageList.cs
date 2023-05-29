namespace Pattern.DTO
{
    public class PageList<T> where T : class
    {
        public PageList(int pageIndex, int pageSize, int totalRecords, List<T> records) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Records = records;
            TotalRecords = totalRecords;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Records { get; set; } = new List<T>();
    }
}
