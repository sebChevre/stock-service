using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StockApi.Models
{
    public class Stock
    {

        public Stock(){}
       
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string BeerId { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [Required,Range(1, 9999)]
        public int StockDisponible { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [Required,Range(1, 9999)]
        public int StockReserve { get; set; }

        public override string ToString()
        {
            return "Id:"+Id+",stockDispo:"+StockDisponible+",beerId:"+BeerId;
        }
    }
}