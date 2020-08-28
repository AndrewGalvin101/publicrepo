using GuildCars.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.Interfaces
{
    public interface IReportsRepository
    {
        IEnumerable<InventoryReport> GetNew();
        IEnumerable<InventoryReport> GetUsed();
        IEnumerable<SalesReport> Search(SalesReportParameters parameters); 
    }
}
