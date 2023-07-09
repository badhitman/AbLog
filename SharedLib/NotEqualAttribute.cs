////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class NotEqualAttribute : ValidationAttribute
{
    private string OtherProperty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public NotEqualAttribute(string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // get other property value
        System.Reflection.PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        object? otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

        // verify values
        if (value?.ToString()?.Equals(otherValue?.ToString()) == true)
            return new ValidationResult(!string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : string.Format("{0} не должен быть равен {1}.", validationContext.MemberName, OtherProperty), new string[] { OtherProperty });
        else
            return ValidationResult.Success;
    }
}
