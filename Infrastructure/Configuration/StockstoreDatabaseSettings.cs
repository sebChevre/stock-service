
namespace StockApi.Infrastructure.Configuration
{
    public class StockstoreDatabaseSettings : IStockstoreDatabaseSettings
    {
        public string StockCollectionName { get; set; }

        public string DatabaseName { get; set; }
    }

    public interface IStockstoreDatabaseSettings
    {
        string StockCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}
