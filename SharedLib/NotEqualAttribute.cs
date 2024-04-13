////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// НЕ:равен (атрибут валидации)
/// </summary>
public class NotEqualAttribute(string otherProperty) : ValidationAttribute
{
    /// <inheritdoc/>    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // get other property value
        System.Reflection.PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
        object? otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

        // verify values
        if (value?.ToString()?.Equals(otherValue?.ToString()) == true)
            return new ValidationResult(!string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : string.Format("{0} не должен быть равен {1}.", validationContext.MemberName, otherProperty), [otherProperty]);
        else
            return ValidationResult.Success;
    }
}