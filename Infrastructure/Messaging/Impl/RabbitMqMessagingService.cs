
using StockApi.Infrastructure.Messaging.Impl.RabbitMq;

namespace BeerApi.Infrastructure.Messaging.Impl
{

    public class RabbitMqMessagingService : MessagingService
    {

        private readonly RabbitMqHandler _rabbitMqHandler;

        public RabbitMqMessagingService(){
            _rabbitMqHandler = new RabbitMqHandler();
        }
        public void Send(string beerId){
           _rabbitMqHandler.Send(beerId);
        }
    }
}