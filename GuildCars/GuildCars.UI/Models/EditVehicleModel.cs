using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Models
{
    public class EditVehicleModel : IValidatableObject
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<SelectListItem> Makes { get; set; }
        public IEnumerable<SelectListItem> Models { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Vehicle.Year.ToString()))
            {
                errors.Add(new ValidationResult("Year is required")); ;
            }
            else if (Vehicle.Year < 2000 || Vehicle.Year > DateTime.Now.Year + 1)
            {
                errors.Add(new ValidationResult($"Year must be between 2000 and {DateTime.Now.Year + 1}")); ;
            }

            if (string.IsNullOrEmpty(Vehicle.MSRP.ToString()))
            {
                errors.Add(new ValidationResult("MSRP is required")); ;
            }
            else if (Vehicle.MSRP <= 0)
            {
                errors.Add(new ValidationResult("MSRP must be a positive number")); ;
            }

            if (string.IsNullOrEmpty(Vehicle.SalePrice.ToString()))
            {
                errors.Add(new ValidationResult("SalePrice is required")); ;
            }
            else if (Vehicle.SalePrice <= 0)
            {
                errors.Add(new ValidationResult("Sale Price must be a positive number")); ;
            }
            else if (Vehicle.SalePrice > Vehicle.MSRP)
            {
                errors.Add(new ValidationResult("Sale Price must not be greater than MSRP")); ;
            }

            if (string.IsNullOrEmpty(Vehicle.VIN))
            {
                errors.Add(new ValidationResult("VIN# is required")); ;
            }

            if (string.IsNullOrEmpty(Vehicle.Description))
            {
                errors.Add(new ValidationResult("Description is required")); ;
            }

            if (string.IsNullOrEmpty(Vehicle.Mileage.ToString()))
            {
                errors.Add(new ValidationResult("Mileage is required")); ;
            }
            else if (Vehicle.TypeID == 1 && Vehicle.Mileage > 1000)
            {
                errors.Add(new ValidationResult("Mileage for a new vehicle must be 1000 or less")); ;
            }
            else if (Vehicle.TypeID == 2 && Vehicle.Mileage <= 1000)
            {
                errors.Add(new ValidationResult("Mileage for a used vehicle must be at least 1000")); ;
            }

            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                var extensions = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

                var extension = Path.GetExtension(ImageUpload.FileName);

                if (!extensions.Contains(extension))
                {
                    errors.Add(new ValidationResult("Image file must be a jpg, png, gif, or jpeg.")); ;
                }
            }

            return errors;
        }
    }
}