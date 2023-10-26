using Microsoft.EntityFrameworkCore;

namespace _26_TranGiaBao_Ass3.Utils
{
    public class Paganation<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public Paganation(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }
        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }
        public static async Task<Paganation<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).
                ToListAsync();

            return new Paganation<T>(items, count, pageIndex, pageSize);
        }
    }
}
