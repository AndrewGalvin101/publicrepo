using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.Data.FileData;
using static FlooringMastery.Data.TestData;

namespace FlooringMastery.BLL
{
    public class Factory
    {
        public static class DataManagerFactory
        {
            public static BLL.DataManager Create()
            {
                string mode = ConfigurationManager.AppSettings["Mode"].ToString();

                switch (mode)
                {
                    case "test":
                        return new BLL.DataManager(new TestDataRepository());
                    case "prod":
                        return new BLL.DataManager(new FileDataRepository());
                    default:
                        throw new Exception("Mode value in app config is not valid");
                } 
            } 
        }
    }
}
