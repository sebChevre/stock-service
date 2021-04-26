using System.Collections.Generic;
using BeerApi.Models;

namespace BeerApi.Services
{
    public interface BeerService
    {
        public List<Beer> GetAllBeers();

        public Beer CreateBeer(Beer beer);

        public void UpdateBeer(Beer beer, string id);

        public void DeleteBeer(string id);

        public Beer GetBeer(string id);
    }
}

