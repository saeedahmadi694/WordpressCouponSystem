using Coupon.Context;
using Coupon.Dtos.Credits;
using Coupon.Models.Entities.CreditAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Coupon.Services;

public class CreditRepository : ICreditRepository
{
    private readonly CouponDataContext _context;
    public CreditRepository(CouponDataContext context)
    {
        _context = context;
    }
    public async Task<Credit?> GetCreditByCode(string code)
    {
        return await _context.Credits
            .Include(r => r.CreditUsages)
            .SingleOrDefaultAsync(c => c.Code == code);
    }
    public async Task<Credit> GetAsync(Expression<Func<Credit, bool>> filter)
    {
        return await _context.Credits
            .Include(r => r.CreditUsages)
            .Where(filter).FirstOrDefaultAsync();
    }

    public async Task<List<CreditDto>> GetAllWithFilter()
    {
        var items = _context.Credits
            .Include(r => r.CreditUsages)
           .OrderByDescending(e => e.Id).AsQueryable();

        return await items.Select(r => new CreditDto(r)).ToListAsync();
    }

    public Task<List<Credit>> GetAllAsync(Expression<Func<Credit, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }


    public Task<Credit?> FirstOrDefaultAsync(Expression<Func<Credit, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<Credit, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<Credit, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<Credit> InsertNew(Credit newEntity)
    {
        throw new NotImplementedException();
    }

    public Task Update(Credit updateEntity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Credit deleteEntity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRang(List<Credit> deleteEntities)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}
