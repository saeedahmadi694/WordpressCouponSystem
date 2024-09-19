using UniBook.Core.Enum;

namespace Coupon.Models.Entities.CreditAggregate;

public abstract class Credit
{
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public virtual CreditType CreditsType { get; protected set; }
    public DateTime? ExpirationDate { get; private set; }
    public UsageRestriction? UsageRestriction { get; private set; }
    public UsageLimit? UsageLimit { get; private set; }
    public bool IsActive { get; private set; }
    public int UsageCount
    {
        get => _creditUsages.Count;
        set { }
    }
    public bool IsValid =>
        IsActive &&
        ExpirationDate.HasValue && ExpirationDate.Value.Date > DateTime.Now.Date &&
        (UsageLimit?.UsageLimitPerCredit.HasValue ?? false && UsageLimit.UsageLimitPerCredit > _creditUsages.Count);

    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate.Value.Date < DateTime.Now.Date;

    private List<CreditUsage> _creditUsages;
    public IReadOnlyCollection<CreditUsage> CreditUsages => _creditUsages.ToList().AsReadOnly();


    private HashSet<int> _allowedProducts;
    public IReadOnlyCollection<int> AllowedProducts => _allowedProducts.ToList().AsReadOnly();


    private HashSet<int> _excludedProducts;
    public IReadOnlyCollection<int> ExcludedProducts => _excludedProducts.ToList().AsReadOnly();


    private HashSet<int> _allowedCategories;
    public IReadOnlyCollection<int> AllowedCategories => _allowedCategories.ToList().AsReadOnly();


    private HashSet<int> _excludedCategories;
    public IReadOnlyCollection<int> ExcludedCategories => _excludedCategories.ToList().AsReadOnly();


    private HashSet<int> _allowedUsers;
    public IReadOnlyCollection<int> AllowedUsers => _allowedUsers.ToList().AsReadOnly();
    protected Credit()
    {
        _allowedProducts = new();
        _excludedProducts = new();
        _allowedCategories = new();
        _excludedCategories = new();
        _allowedUsers = new();
        _creditUsages = new();
        IsActive = true;
    }
    protected Credit(string code, string? description, DateTime? expirationDate, CreditType creditsType) : this()
    {
        SetCode(code);
        CreditsType = creditsType;
        SetDescription(description);
        SetExpirationDate(expirationDate);
    }
    public abstract Credit SetValue(object value);

    public Credit SetCode(string code)
    {
        Code = code;
        return this;
    }
    public Credit SetDescription(string? description)
    {
        Description = description;
        return this;
    }
    public Credit SetExpirationDate(DateTime? expirationDate)
    {
        ExpirationDate = expirationDate;
        return this;
    }
    public Credit SetPerCredit(int? usageLimitPerCredit)
    {
        if (UsageLimit is null)
            UsageLimit = new UsageLimit(usageLimitPerCredit, null);
        else
            UsageLimit = UsageLimit with { UsageLimitPerCredit = usageLimitPerCredit };
        return this;
    }
    public Credit SetPerUser(int? usageLimitPerUser)
    {
        if (UsageLimit is null)
            UsageLimit = new UsageLimit(null, usageLimitPerUser);
        else
            UsageLimit = UsageLimit with { UsageLimitPerUser = usageLimitPerUser };
        return this;
    }

    public Credit Active()
    {
        IsActive = true;
        return this;
    }
    public Credit Deactive()
    {
        IsActive = false;
        return this;
    }



    #region For  Usages
    public Credit AddUsage(int userId)
    {
        var item = _creditUsages.SingleOrDefault(r => r.UserId == userId);
        if (item is null)
        {
            _creditUsages.Add(new CreditUsage(userId));
        }
        else
        {
            item.IncreaseUsageCount();
        }
        return this;
    }

    internal CreditUsage GetUsage(int userId)
    {
        try
        {
            var item = _creditUsages.SingleOrDefault(r => r.UserId == userId);
            if (item is null)
            {
                throw new Exception("item not found");
            }
            return item;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException($"Multiple usages found with userId {userId}");
        }

    }

    public Credit RemoveUsage(int userId)
    {
        var item = GetUsage(userId);
        _creditUsages.Remove(item);
        return this;
    }


    public Credit IncreaseUsageCount(int userId)
    {
        var item = GetUsage(userId);
        item.IncreaseUsageCount();
        return this;
    }
    public Credit IncreaseUsageLimit(int userId)
    {
        var item = GetUsage(userId);
        item.IncreaseUsageLimit();
        return this;
    }
    public bool CanUserUseCredit(int userId)
    {
        var item = _creditUsages.SingleOrDefault(r => r.UserId == userId);

        if (item is null) return true;

        if (item.UsageCount < item.UsageLimit) return true;

        return false;
    }
    #endregion

}
