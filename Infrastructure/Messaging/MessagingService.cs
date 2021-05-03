

namespace BeerApi.Infrastructure.Messaging
{

    public interface MessagingService
    {

        public void Send(string beerId);
    }
}