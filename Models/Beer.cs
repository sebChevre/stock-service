using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BeerApi.Models
{
    public class Beer
    {
       
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required, MaxLength(30), RegularExpression(@"^[a-zA-Zàâçéèêëîïôûùüÿñæœ0-9''-'\s]{1,40}$")] ///^[a-zàâçéèêëîïôûùüÿñæœ .-]*$/i
        public string BeerName { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [Required,Range(1.00, 99.99)]
        public decimal Price { get; set; }

        [Required, MaxLength(30), RegularExpression(@"^[a-zA-Zàâçéèêëîïôûùüÿñæœ0-9''-'\s]{1,40}$")] ///^[a-zàâçéèêëîïôûùüÿñæœ .-]*$/i
        public string Category { get; set; }

        [Required, MaxLength(30), RegularExpression(@"^[a-zA-Zàâçéèêëîïôûùüÿñæœ0-9''-'\s]{1,40}$")] ///^[a-zàâçéèêëîïôûùüÿñæœ .-]*$/i
        public string Manufacturer { get; set; }

        public override string ToString()
        {
            return "beerName:"+BeerName+",price:"+Price+",category:"+Category+",manufacturer:"+Manufacturer;
        }
    }
}