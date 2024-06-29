using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CouncilsManagmentSystem.Attributes;
using Microsoft.AspNetCore.Http;

namespace CouncilsManagmentSystem.DTOs
{
    public class AddConfirmAttendanceDto
    {
        //[Required(ErrorMessage = "PDF file is required.")]
        //[AllowedExtensions(".pdf")]
        //public IFormFile? Pdf { get; set; }

        [Required(ErrorMessage = "Please specify whether the member is attending.")]
        public bool IsAttending { get; set; }

        [RequiredIfFalse(nameof(IsAttending), ErrorMessage = "Reason for non-attendance is required.")]
        public string? ReasonNonAttendance { get; set; }

        [Required(ErrorMessage = "Council ID is required.")]
        public int CouncilId { get; set; }

    }


    public class RequiredIfFalseAttribute : ValidationAttribute
    {
        private readonly string _propertyName;

        public RequiredIfFalseAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
            {
                return new ValidationResult($"Unknown property {_propertyName}");
            }

            var propertyValue = property.GetValue(validationContext.ObjectInstance);
            if (propertyValue is bool boolValue && boolValue == false && string.IsNullOrEmpty((string)value))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}