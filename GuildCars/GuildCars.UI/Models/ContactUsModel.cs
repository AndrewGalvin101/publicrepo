using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class ContactUsModel: IValidatableObject
    {
        public Contact contact { get; set; }
        public string insert { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(contact.Name))
            {
                errors.Add(new ValidationResult("Name is required")); ;
            }

            if (string.IsNullOrEmpty(contact.Email) && string.IsNullOrEmpty(contact.Phone))
            {
                errors.Add(new ValidationResult("Email or Phone is required")); ;
            }

            if (string.IsNullOrEmpty(contact.Message))
            {
                errors.Add(new ValidationResult("Message is required")); ;
            }

            return errors;
        }
    }
}