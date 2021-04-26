using BeerApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BeerApi.Infrastructure{

    public interface BeerRepository{

        public List<Beer> GetAllBeers();

        public Beer CreateBeer(Beer beer);

        public void UpdateBeer(Beer beer, string id);

        public void DeleteBeer(string id);

        public Beer getBiereById(string id);
    }
}