using Coupon.Dtos.Common;
using Coupon.Dtos.Credits;
using Coupon.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Coupon.Feature.CreditFeature;

namespace Coupon.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CreditController : ControllerBase
{
    private readonly IMediator _mediator;
    public CreditController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetCreditListQuery { }, cancellationToken);
        return Ok(new CreditViewModel(items));
    }

    [HttpPost]
    public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditCreditDto input, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
            await _mediator.Send(new CreateCreditCommand { CreateOrEditCreditDto = input }, cancellationToken);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }




    [HttpPut]
    public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditCreditDto voucherDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
            await _mediator.Send(new EditCreditCommand { CreateOrEditCreditDto = voucherDto }, cancellationToken);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteCreditCommand { Id = id }, cancellationToken);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }


}
