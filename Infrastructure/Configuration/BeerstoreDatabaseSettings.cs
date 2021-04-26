using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerApi.Infrastructure.Configuration
{
    public class BeerstoreDatabaseSettings : IBeerstoreDatabaseSettings
    {
        public string BeerCollectionName { get; set; }

        public string DatabaseName { get; set; }
    }

    public interface IBeerstoreDatabaseSettings
    {
        string BeerCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}
