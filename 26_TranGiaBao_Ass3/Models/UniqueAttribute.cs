using System.ComponentModel.DataAnnotations;

namespace _26_TranGiaBao_Ass3.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var emailProperty = instance.GetType().GetProperty("Email");
            var emailValue = emailProperty.GetValue(instance);

            if (emailValue.ToString() == value.ToString())
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The value must be unique.");
        }
    }
}