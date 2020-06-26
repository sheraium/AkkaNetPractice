using System;
using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using ReactiveStock.ExternalServices;

namespace ReactiveStock.ActorModel.Actors
{
    public class StockPriceLookupActor : ReceiveActor
    {
        private readonly IStockPriceServiceGateway _stockPriceServiceGateway;

        public StockPriceLookupActor(IStockPriceServiceGateway stockPriceServiceGateway)
        {
            _stockPriceServiceGateway = stockPriceServiceGateway;

            Receive<RefreshStockPriceMessage>(message => LookupStockPrice(message));
        }

        private void LookupStockPrice(RefreshStockPriceMessage message)
        {
            var latestPrice = _stockPriceServiceGateway.GetLatestPrice(message.StockSymbol);
            Sender.Tell(new UpdatedStockPriceMessage(latestPrice, DateTime.Now));
        }
    }
}