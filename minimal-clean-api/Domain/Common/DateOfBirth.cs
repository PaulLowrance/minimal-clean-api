using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace MinimalCleanApi.Domain.Common;

public class DateOfBirth : ValueOf<DateOnly, DateOfBirth>
{
    protected override void Validate()
    {
        if(Value < DateOnly.FromDateTime(DateTime.Now)) return;
        const string msg = "Date of birth cannot be in the future";
        throw new ValidationException(msg, new[]
        {
            new ValidationFailure(nameof(DateOfBirth), msg)
        });
    }
}