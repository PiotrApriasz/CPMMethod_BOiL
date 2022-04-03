using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CPMMethod.Logic.Validators;

public class CustomValidationActivityId : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult("Id Aktywności jest wymagane");

        return !int.TryParse(value.ToString(), out var id) ?
            new ValidationResult("Id Aktywności powinno być liczbą") : ValidationResult.Success;
    }
}