using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace MinimalCleanApi.Domain.Common;

public class NamePart : ValueOf<string, NamePart>
{
    private static readonly Regex NameRegex = new("^[a-z ,.'-]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    protected override void Validate()
    {
        if (NameRegex.IsMatch(Value)) return;
        var msg = $"{Value} is not a valid name part";
        throw new ValidationException(msg, new[]
        {
            new ValidationFailure(nameof(NamePart), msg)
        });
    }
}