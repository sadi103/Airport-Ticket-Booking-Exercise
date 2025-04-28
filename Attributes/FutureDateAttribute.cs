using System.ComponentModel.DataAnnotations;

namespace FTS.AirportTicketBookingExercise.Attributes
{
    [AttributeUsage(AttributeTargets.Property
        | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    sealed class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
            : base("The date must be in the future") { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime date && date > DateTime.Now)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} must be set to future dates");
        }
    }
}
