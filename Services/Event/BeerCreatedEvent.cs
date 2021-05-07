
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BeerApi.Services.Event
{

    public class BeerCreatedEvent
    {


        public BeerCreatedEvent () {
            EventType = "beer-created";
        }
        public string EventType { get; set; }

        public string BeerId { get; set; }

        public string Location { get; set; }


        public string AsJson(){

            return JsonSerializer.Serialize(this);
        }
    }

}