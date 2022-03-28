using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CPMMethod.BlazorWasmClient.Validators;

public class CustomValidationActivityId : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult("Id Aktywności jest wymagane");
        
        var id = value.ToString();
        var errorCounterString = Regex.Matches(id,@"[a-zA-Z]").Count;
        var result = id.Any(char.IsLetter);

        if (id.Length > 1 || !result)
        {
            return new ValidationResult("Id Aktywności powinno być pojedynczą literą");
        }

        return ValidationResult.Success;

    }
}