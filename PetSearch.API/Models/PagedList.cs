namespace PetSearch.API.Models;

public class PagedList<T>
{
    public List<T> Items { get; private set; }
    public PaginationMetaData Pagination { get; private set; }
    
    public bool HasPrevious => (Pagination.CurrentPage > 1);
    public bool HasNext => (Pagination.CurrentPage < Pagination.TotalPages);
    
    public PagedList(List<T> items, PaginationMetaData paginationMetaData)
    {
        Items = items;
        Pagination = paginationMetaData;
    }
}