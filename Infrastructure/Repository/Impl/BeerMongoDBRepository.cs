using BeerApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using BeerApi.Infrastructure.Repository.Impl.MongoDB;
using BeerApi.Infrastructure.Configuration;
using MongoDB.Bson;

namespace BeerApi.Infrastructure.Repository.Impl
{

    public class BeerMongDBRepository : BeerRepository 
    {
        private readonly IMongoCollection<Beer> _beers;
        private readonly MongoDBHandler _mongoDBHandler;

        public BeerMongDBRepository(IBeerstoreDatabaseSettings settings){
            _mongoDBHandler = new MongoDBHandler(settings);

             _beers =  _mongoDBHandler.GetDataBase().GetCollection<Beer>(settings.BeerCollectionName);
        }
        public List<Beer> GetAllBeers() =>
            _beers.Find(beer => true).ToList();

        public Beer CreateBeer(Beer beer)
        {
            _beers.InsertOne(beer);
            return beer;
        }

        public void UpdateBeer(Beer beer, string id){

            //|| !id.Equals(beer.Id
            if(getBiereById(id) == null){
                throw new System.ArgumentOutOfRangeException("No entity found with id :" + id);
            }
            

            _beers.ReplaceOne(b=> b.Id == beer.Id,beer);
        }

         public void DeleteBeer(string id){

             if(getBiereById(id) == null){
                throw new System.ArgumentOutOfRangeException("No entity found with id :" + id);
            }

            _beers.DeleteOne(b=> b.Id == id);
        }

        public Beer getBiereById(string id){
            
            if(id.Length != 24){
                throw new System.ArgumentOutOfRangeException("The size of the id must be 24 hex digit");
            }
            
            Beer beer = _beers.Find(x => x.Id == id).SingleOrDefault();

            return beer;
        }

    }
}























