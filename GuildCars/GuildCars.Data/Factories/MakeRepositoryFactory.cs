using GuildCars.Data.ADO;
using GuildCars.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.Factories
{
    public class MakeRepositoryFactory
    {
        public static IMakeRepository GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "PROD":
                    return new MakeRepositoryADO();
                case "QA":
                // return new MakeRepositoryQA():
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}
