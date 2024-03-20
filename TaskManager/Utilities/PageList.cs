namespace TaskManager.Utilities
{
    public class PageList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;


        public PageList(List<T> Items , int count , int index , int pageSize)
        {
            this.PageIndex = index;

            this.TotalPages = (int)Math.Ceiling( count/((double) pageSize));

            this.AddRange(Items); 


        }
        
    }
}
