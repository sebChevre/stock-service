using System.Collections.Generic;
using BeerApi.Models;
using BeerApi.Infrastructure;

namespace BeerApi.Services.Impl
{
    public class BeerServiceImpl : BeerService
    {
        
        private readonly BeerRepository _beerRepository;

        public BeerServiceImpl(BeerRepository beerRepository){
            _beerRepository = beerRepository;
        }
        public List<Beer> GetAllBeers(){
            return _beerRepository.GetAllBeers();
        }
        public Beer CreateBeer(Beer beer){
            return _beerRepository.CreateBeer(beer);
        }

        public void UpdateBeer(Beer beer, string id){
            _beerRepository.UpdateBeer(beer, id);
        }

        public void DeleteBeer(string id){
            _beerRepository.DeleteBeer(id);
        }

        public Beer GetBeer(string id){
            return _beerRepository.getBiereById(id);
        }
    }
}