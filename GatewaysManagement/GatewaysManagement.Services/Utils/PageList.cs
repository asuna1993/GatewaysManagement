using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaysManagement.Services.Utils
{
    public class PageList<T> : List<T>
    {
        private int CurrentPage { get; set; }
        private int TotalPages { get; set; }
        private int PageSize { get; set; }
        private int TotalCount { get; set; }
        private bool HasPrevius => (CurrentPage > 1);
        private bool HasNext => (CurrentPage < TotalPages);

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return await Task.FromResult(new PageList<T>(items, count, pageNumber, pageSize));
        }

        public object GetPaginationData
        {
            get
            {
                return new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentpage = CurrentPage,
                    totalPages = TotalPages,
                    hasPrevius = HasPrevius,
                    hasNext = HasNext
                };
            }
        }
    }
}