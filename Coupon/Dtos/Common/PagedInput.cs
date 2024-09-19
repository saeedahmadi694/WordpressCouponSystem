namespace BookHouse.Awards.Infrastructure.Dtos.Common;
public abstract record PagedInput(int PageNumber = 1, int PageSize = 10)
{
    public int SkipCount => (PageNumber - 1) * PageSize;
}
