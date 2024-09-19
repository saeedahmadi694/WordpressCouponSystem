namespace Coupon.Dtos.Common;


public class ReturnedDto
{
    public object? Result { get; set; }
    public int Status { get; set; }
    public string Target { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

