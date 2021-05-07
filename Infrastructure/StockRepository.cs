using StockApi.Models;
using System.Collections.Generic;

namespace StockApi.Infrastructure{

    public interface StockRepository{

        public List<Stock> GetAllStock();

        public Stock CreateStock(Stock stock);

        public void UpdateStock(Stock stock,string stockId);

        public void DeleteStock(string id);

        public Stock getStockByBiereId(string id);
    }
}