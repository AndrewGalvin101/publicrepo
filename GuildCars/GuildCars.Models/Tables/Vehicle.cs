using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Tables
{
    public class Vehicle
    {
        public int ID { get; set; }
        public int MakeID { get; set; }
        public int ModelID { get; set; }
        public int TypeID { get; set; }
        public int BodyStyleID { get; set; }
        public int Year { get; set; }
        public int TransmissionID { get; set; }
        public int ColorID { get; set; }
        public int InteriorID { get; set; }
        public int Mileage { get; set; }
        public string VIN { get; set; }
        public decimal MSRP { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public bool Featured { get; set; }
        public int? PurchaseID { get; set; }
    }

    public class Types
    {
        public int TypeID { get; set; }
        public string Type { get; set; }
    }

    public class BodyStyles
    {
        public int BodyStyleID { get; set; }
        public string BodyStyle { get; set; }
    }

    public class Transmissions
    {
        public int TransmissionID { get; set; }
        public string Transmission { get; set; }
    }

    public class Colors
    {
        public int ColorID { get; set; }
        public string Color { get; set; }
    }

    public class Interiors
    {
        public int InteriorID { get; set; }
        public string Interior { get; set; }
    }
}
