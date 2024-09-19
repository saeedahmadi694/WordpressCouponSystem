using Coupon.Dtos.Credits;
using Coupon.Models.Entities.CreditAggregate;
using System.Linq.Expressions;

namespace Coupon.Services;

public interface ICreditRepository
{
    Task<Credit?> GetCreditByCode(string code);
    Task<List<CreditDto>> GetAllWithFilter();

    Task<List<Credit>> GetAllAsync(Expression<Func<Credit, bool>>? filter = null);
    Task<Credit> GetAsync(Expression<Func<Credit, bool>> filter);
    Task<Credit?> FirstOrDefaultAsync(Expression<Func<Credit, bool>>? filter = null);
    Task<int> CountAsync(Expression<Func<Credit, bool>> filter);
    Task<bool> AnyAsync(Expression<Func<Credit, bool>> filter);
    Task<Credit> InsertNew(Credit newEntity);
    Task Update(Credit updateEntity);
    Task Delete(int id);
    Task Delete(Credit deleteEntity);
    Task DeleteRang(List<Credit> deleteEntities);

    void SaveChanges();
    Task SaveChangesAsync();
}
