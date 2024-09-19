using BookHouse.Awards.Infrastructure.Dtos.Common;
using Coupon.Dtos.Credits;

namespace Coupon.ViewModels;

public record CreditViewModel(IReadOnlyList<CreditDto> items)
{
}