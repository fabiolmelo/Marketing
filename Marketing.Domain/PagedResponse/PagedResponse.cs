using System;
using System.Collections.Generic;

namespace Marketing.Domain.PagedResponse
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public T Dados { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            Dados = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        }
    }
}
