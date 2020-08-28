using GuildCars.Models;
using GuildCars.Models.Queries;
using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class PurchaseView : IValidatableObject
    {
        public PurchaseDetails purchaser { get; set; }
        public VehicleDetailsItem vehicle { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(purchaser.Email) && string.IsNullOrEmpty(purchaser.Phone))
            {
                errors.Add(new ValidationResult("Email or Phone is required")); ;
            }

            if (string.IsNullOrEmpty(purchaser.Name))
            {
                errors.Add(new ValidationResult("Name is required")); ;
            }

            if (string.IsNullOrEmpty(purchaser.Street1))
            {
                errors.Add(new ValidationResult("Street1 is required")); ;
            }

            if (string.IsNullOrEmpty(purchaser.City))
            {
                errors.Add(new ValidationResult("City is required")); ;
            }

            if (string.IsNullOrEmpty(purchaser.ZipCode) || (purchaser.ZipCode.Length != 5))
            {
                errors.Add(new ValidationResult("Zipcode must be 5 digits")); ;
            }

            if (purchaser.PurchasePrice == 0) 
            {
                errors.Add(new ValidationResult("Purchase Price is required")); ;
            }

            if (purchaser.PurchasePrice < decimal.Multiply(vehicle.SalePrice, (decimal).95))
            {
                errors.Add(new ValidationResult("Purchase Price cannot be less than 95% of Sale Price")); ;
            }

            if (purchaser.PurchasePrice > vehicle.MSRP)
            {
                errors.Add(new ValidationResult("Purchase Price cannot be greater than MSRP.")); ;
            }

            return errors;
        }
    }
}