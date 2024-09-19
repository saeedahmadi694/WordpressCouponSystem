namespace BookHouse.Awards.Infrastructure.Dtos.Common;
public record PagedDto<TData>(int TotalCount, List<TData> Items)
where TData : class
{
}

