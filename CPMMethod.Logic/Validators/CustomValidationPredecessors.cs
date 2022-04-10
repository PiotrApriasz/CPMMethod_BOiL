using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CPMMethod.Logic.Validators;

public class CustomValidationPredecessors : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult("Pole jest wymagane");

        var predecessors = value.ToString();
        
        if (string.IsNullOrEmpty(predecessors)) return new ValidationResult("Pole jest wymagane");

        var isValid = Regex.Match(predecessors, @"^[0-9,.]*$");

        if (predecessors == "-" || isValid.Success)
            return ValidationResult.Success;

        return new ValidationResult("Wymagany format dla tego pola to '-' dla braku poprzedników oraz 'id,id,...' dla istnienia poprzedników");
    }
}