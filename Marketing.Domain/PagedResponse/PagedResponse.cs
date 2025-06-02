namespace Marketing.Domain.PagedResponse
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public List<T> Dados { get; init; }

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Dados = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        }
    }
}
