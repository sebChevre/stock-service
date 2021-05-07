using System.Collections.Generic;
using StockApi.Models;
using StockApi.Infrastructure;

namespace StockApi.Services.Impl
{
    public class StockServiceImpl : StockService
    {
        
        private readonly StockRepository _StockRepository;

        public StockServiceImpl(StockRepository StockRepository){
            _StockRepository = StockRepository;
        }
        public List<Stock> GetAllStocks(){
            return _StockRepository.GetAllStock();
        }
        public Stock CreateStock(Stock stock){
            return _StockRepository.CreateStock(stock);
        }

        public void UpdateStock(Stock stock,string id){
            _StockRepository.UpdateStock(stock,id);
        }

        public void DeleteStock(string id){
            _StockRepository.DeleteStock(id);
        }

        public Stock GetStockByBiereId(string id){
            return _StockRepository.getStockByBiereId(id);
        }

    }
}