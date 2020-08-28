using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public class Requests
    {
        public class FileLookupRequest
        {
            public bool success;
            public List<Order> orders;
            public string message;
            public string path;
        }

        public class ProductTypeRequest
        {
            public bool success;
            public string[] rows;
        }
    }
}
