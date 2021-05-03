using System.Collections.Generic;
using BeerApi.Models;
using BeerApi.Infrastructure.Repository;
using BeerApi.Infrastructure.Messaging;

namespace BeerApi.Services.Impl
{
    public class BeerServiceImpl : BeerService
    {
        
        private readonly BeerRepository _beerRepository;
        private readonly MessagingService _messagingService;

        public BeerServiceImpl(BeerRepository beerRepository,MessagingService messagingService){
            _beerRepository = beerRepository;
            _messagingService = messagingService;

        }
        public List<Beer> GetAllBeers(){
            return _beerRepository.GetAllBeers();
        }
        public Beer CreateBeer(Beer beer){
            Beer b = _beerRepository.CreateBeer(beer);
            _messagingService.Send(beer.Id); 
            return b;
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