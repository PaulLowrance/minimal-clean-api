using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace MinimalCleanApi.Domain.Membership;

public class DateOfMembership: ValueOf<DateOnly, DateOfMembership>
{
    protected override void Validate()
    {
        if (Value < DateOnly.FromDateTime(DateTime.Now)) return;
        const string msg = "Date of membership cannot be in the future";
        throw new ValidationException(msg, new[]
        {
            new ValidationFailure(nameof(DateOfMembership), msg)
        });
    }
}