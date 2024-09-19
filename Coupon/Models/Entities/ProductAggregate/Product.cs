namespace UniBook.Core.Entities.ProductAggregate;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid UId { get; set; }
    public decimal Price { get; set; }
    public string? Features { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public int ViewCount { get; set; }

}
