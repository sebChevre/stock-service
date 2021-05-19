using StockApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using StockApi.Infrastructure.MongoDB;
using StockApi.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;

namespace StockApi.Infrastructure.Impl
{

    public class StockMongoDbRepository : StockRepository 
    {
        private readonly IMongoCollection<Stock> _stocks;
        private readonly MongoDBHandler _mongoDBHandler;
        private readonly ILogger<StockMongoDbRepository> _logger;
        public StockMongoDbRepository(ILogger<StockMongoDbRepository> logger,IStockstoreDatabaseSettings settings){
            _mongoDBHandler = new MongoDBHandler(settings);
            _logger = logger;

             _stocks =  _mongoDBHandler.GetDataBase().GetCollection<Stock>(settings.StockCollectionName);
        }
    

        List<Stock> StockRepository.GetAllStock()
        {
            return _stocks.Find(stock => true).ToList();
        }

        Stock StockRepository.CreateStock(Stock stock)
        {
           
            var filter = Builders<Stock>.Filter.Eq("BeerId", stock.BeerId);
            
            var isBeerAlreadyPresent = _stocks.Find(filter).FirstOrDefault();

            if(isBeerAlreadyPresent != null){
                _logger.LogWarning("Stock already present for beer: {}",stock.BeerId);
                throw new System.Exception("Stock already exist for beer: " + stock.BeerId);

            }else{
                _stocks.InsertOne(stock); 
                _logger.LogInformation("Stock inserted for beer: {0}",stock.BeerId);
            }
            
            return stock;
        }

        void StockRepository.UpdateStock(Stock stock, string stockId)
        {
            if(getStockById(stockId) == null){
                throw new System.ArgumentOutOfRangeException("No entity found with id :" + stockId);
            }
            _stocks.ReplaceOne(s => s.Id == stock.Id,stock);
        }

        void StockRepository.DeleteStock(string id)
        {
             if(getStockById(id) == null){
                throw new System.ArgumentOutOfRangeException("No entity found with id :" + id);
            }

            _stocks.DeleteOne(b=> b.Id == id);
        }
        

        

        public Stock getStockById(string id){
            
            if(id.Length != 24){
                throw new System.ArgumentOutOfRangeException("The size of the id must be 24 hex digit");
            }
            
            Stock stock = _stocks.Find(x => x.Id == id).SingleOrDefault();

            return stock;
        }

        public Stock getStockByBiereId(string id){
            
            if(id.Length != 24){
                throw new System.ArgumentOutOfRangeException("The size of the id must be 24 hex digit");
            }
            
            Stock stock = _stocks.Find(stock => stock.BeerId == id).SingleOrDefault();

            return stock;
        }
    }
}























