using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Revenues;

public class RevenueValidator: AbstractValidator<RequestRevenueJson> 
{
    public RevenueValidator()
    {
        RuleFor(expense => expense.Title)
            .NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED)
            .MaximumLength(80).WithMessage(ResourceErrorMessages.TITLE_TOO_LONG);

        RuleFor(expense => expense.Description)
            .MaximumLength(300).WithMessage(ResourceErrorMessages.DESCRIPTION_TOO_LONG);

        RuleFor(expense => expense.OccurredAt)
            .Must(date => date != DateTime.MinValue).WithMessage(ResourceErrorMessages.OCCURRED_AT_REQUIRED)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.REVENUES_CANNOT_BE_FOR_THE_FUTURE);

        RuleFor(expense => expense.Amount)
            .NotNull().WithMessage(ResourceErrorMessages.AMOUNT_REQUIRED)
            .GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);

        RuleFor(expense => expense.PaymentType)
            .IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}

