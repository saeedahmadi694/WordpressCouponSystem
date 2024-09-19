using Coupon.Dtos.Credits;
using Coupon.Models.Entities.CreditAggregate;
using Coupon.Services;
using MediatR;
using UniBook.Core.Enum;

namespace Coupon.Feature;

public class CreditFeature
{
    #region CQRS

    #region Create Credit
    public class CreateCreditCommand : IRequest
    {
        public CreateOrEditCreditDto CreateOrEditCreditDto { get; set; }
    }

    public class CreateCreditCommandHandler : IRequestHandler<CreateCreditCommand>
    {

        private readonly ICreditRepository _creditRepository;
        public CreateCreditCommandHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task Handle(CreateCreditCommand request, CancellationToken cancellationToken)
        {
            if (await _creditRepository.AnyAsync(r => r.Code == request.CreateOrEditCreditDto.Code))
                throw new Exception();

            try
            {
                Credit credit = request.CreateOrEditCreditDto.CreditsType switch
                {
                    CreditType.Percentage => new Percentage(request.CreateOrEditCreditDto.Code, request.CreateOrEditCreditDto.Value, request.CreateOrEditCreditDto.Description, request.CreateOrEditCreditDto.ExpirationDate),
                    CreditType.FixedProduct => new FixedProduct(request.CreateOrEditCreditDto.Code, request.CreateOrEditCreditDto.Value, request.CreateOrEditCreditDto.Description, request.CreateOrEditCreditDto.ExpirationDate),
                    CreditType.FixedCart => new FixedCart(request.CreateOrEditCreditDto.Code, request.CreateOrEditCreditDto.Value, request.CreateOrEditCreditDto.Description, request.CreateOrEditCreditDto.ExpirationDate),
                    _ => throw new Exception("error")
                };

                credit.SetPerUser(request.CreateOrEditCreditDto.UsageLimit?.UsageLimitPerUser);
                credit.SetPerCredit(request.CreateOrEditCreditDto.UsageLimit?.UsageLimitPerCredit);

                await _creditRepository.InsertNew(credit);
                await _creditRepository.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }

        }
    }

    #endregion

    #region Edit Credit

    public class EditCreditCommand : IRequest
    {
        public CreateOrEditCreditDto CreateOrEditCreditDto { get; set; }

    }

    public class EditCreditCommandHandler : IRequestHandler<EditCreditCommand>
    {
        private readonly ICreditRepository _creditRepository;
        public EditCreditCommandHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task Handle(EditCreditCommand request, CancellationToken cancellationToken)
        {
            var item = await _creditRepository.GetAsync(r => r.Id == request.CreateOrEditCreditDto.Id.Value);
            if (item is null)
                throw new Exception();


            if (item.Code != request.CreateOrEditCreditDto.Code && await _creditRepository.AnyAsync(r => r.Code == request.CreateOrEditCreditDto.Code))
                throw new Exception();


            try
            {
                item.SetCode(request.CreateOrEditCreditDto.Code);
                item.SetDescription(request.CreateOrEditCreditDto.Description);
                item.SetExpirationDate(request.CreateOrEditCreditDto.ExpirationDate);
                item.SetPerCredit(request.CreateOrEditCreditDto.UsageLimit?.UsageLimitPerCredit);
                item.SetPerUser(request.CreateOrEditCreditDto.UsageLimit?.UsageLimitPerUser);
                item.SetValue(request.CreateOrEditCreditDto.Value);

                if (request.CreateOrEditCreditDto.IsActive) item.Active();
                else item.Deactive();

                await _creditRepository.Update(item);
                await _creditRepository.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }

        }
    }

    #endregion

    #region Delete Credit

    public class DeleteCreditCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCreditCommand>
    {
        private readonly ICreditRepository _couponRepository;
        public DeleteCouponCommandHandler(ICreditRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task Handle(DeleteCreditCommand request, CancellationToken cancellationToken)
        {
            try
            {

                await _couponRepository.Delete(request.Id);
                await _couponRepository.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }


        }
    }

    #endregion

    #region Get Credit Detail

    public class GetCreditDetailQuery : IRequest<Credit>
    {
        public int CreditId { get; set; }
    }

    public class GetCreditDetailQueryHandler : IRequestHandler<GetCreditDetailQuery, Credit>
    {
        private readonly ICreditRepository _creditRepository;
        public GetCreditDetailQueryHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task<Credit> Handle(GetCreditDetailQuery request, CancellationToken cancellationToken)
        {
            var item = await _creditRepository.GetAsync(r => r.Id == request.CreditId);
            if (item == null || item is not FixedCart credit) throw new Exception();

            return credit;
        }
    }

    #endregion

    #region Validation Credit

    public record ValidationCreditQuery(string Code, int UserId) : IRequest<ValidationCreditDto>
    {
    }

    public class ValidationCreditQueryHandler : IRequestHandler<ValidationCreditQuery, ValidationCreditDto>
    {
        private readonly ICreditRepository _creditRepository;
        public ValidationCreditQueryHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task<ValidationCreditDto> Handle(ValidationCreditQuery request, CancellationToken cancellationToken)
        {
            var credit = await _creditRepository.GetCreditByCode(request.Code);

            if (credit == null)
            {
                return new ValidationCreditDto("CreditNotFound", request.Code, 404);
            }
            else if (!credit.IsActive)
            {
                return new ValidationCreditDto("CreditNotActive", request.Code, 300);
            }
            else if (credit.IsExpired)
            {
                return new ValidationCreditDto("CreditExpireDate", request.Code, 300);
            }
            else if (!credit.CanUserUseCredit(request.UserId))
            {
                return new ValidationCreditDto("CreditUsageLimit", request.Code, 300);
            }
            else
                return new ValidationCreditDto("CreditSuccess", request.Code, 200);
        }
    }

    #endregion

    #region Get Credit Detail For Edit

    public class GetCreditDetailForEditQuery : IRequest<CreateOrEditCreditDto>
    {
        public int CreditId { get; set; }
    }

    public class GetCreditDetailForEditQueryHandler : IRequestHandler<GetCreditDetailForEditQuery, CreateOrEditCreditDto>
    {
        private readonly ICreditRepository _creditRepository;
        public GetCreditDetailForEditQueryHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task<CreateOrEditCreditDto> Handle(GetCreditDetailForEditQuery request, CancellationToken cancellationToken)
        {
            var credit = await _creditRepository.GetAsync(r => r.Id == request.CreditId);
            if (credit == null) throw new Exception();
            return new CreateOrEditCreditDto(credit);
        }
    }

    #endregion

    #region Get Credit List
    public class GetCreditListQuery : IRequest<IReadOnlyList<CreditDto>>
    {
    }

    public class GetCreditListQueryHandler : IRequestHandler<GetCreditListQuery, IReadOnlyList<CreditDto>>
    {
        private readonly ICreditRepository _creditRepository;
        public GetCreditListQueryHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }
        public async Task<IReadOnlyList<CreditDto>> Handle(GetCreditListQuery request, CancellationToken cancellationToken)
        {
            return await _creditRepository.GetAllWithFilter();
        }
    }
    #endregion

    #endregion


}
