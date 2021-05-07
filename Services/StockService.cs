using System.Collections.Generic;
using StockApi.Models;

namespace StockApi.Services
{
    public interface StockService
    {
        public List<Stock> GetAllStocks();

        public Stock CreateStock(Stock stock);

        public void UpdateStock(Stock stock,string id);

        public void DeleteStock(string id);

        public Stock GetStockByBiereId(string id);
    }
}

