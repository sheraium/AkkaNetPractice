using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.DI.Core;
using ReactiveStock.ActorModel.Messages;

namespace ReactiveStock.ActorModel.Actors
{
    public class StockActor : ReceiveActor
    {
        private readonly string _stockSymbol;
        private readonly HashSet<IActorRef> _subscribers;
        private readonly IActorRef _priceLookupChild;
        private decimal _stockPrice;
        private ICancelable _priceRefreshing;

        public StockActor(string stockSymbol)
        {
            _stockSymbol = stockSymbol;
            _subscribers = new HashSet<IActorRef>();
            _priceLookupChild = Context.ActorOf(Context.DI().Props<StockPriceLookupActor>());
            Receive<SubscribeToNewStockPricesMessage>(message => _subscribers.Add(message.Subscriber));
            Receive<UnSubscribeFromNewStockPricesMessage>(message => _subscribers.Remove(message.Subscriber));
            Receive<RefreshStockPriceMessage>(message => _priceLookupChild.Tell(message));
            Receive<UpdatedStockPriceMessage>(message =>
            {
                _stockPrice = message.Price;
                var stockPriceMessage = new StockPriceMessage(_stockSymbol, _stockPrice, message.Date);
                foreach (var subscriber in _subscribers)
                {
                    subscriber.Tell(stockPriceMessage);
                }
            });
        }

        protected override void PreStart()
        {
            _priceRefreshing = Context.System
                .Scheduler
                .ScheduleTellRepeatedlyCancelable(
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1),
                    Self,
                    new RefreshStockPriceMessage(_stockSymbol),
                    Self);
        }

        protected override void PostStop()
        {
            _priceRefreshing.Cancel(false);
            base.PostStop();
        }
    }
}