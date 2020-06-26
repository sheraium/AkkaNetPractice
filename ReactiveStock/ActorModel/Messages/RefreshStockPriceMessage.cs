using System;

namespace ReactiveStock.ActorModel.Messages
{
    public class RefreshStockPriceMessage
    {
        public string StockSymbol { get; private set; }

        public RefreshStockPriceMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }

    public class UpdatedStockPriceMessage
    {
        public decimal Price { get; private set; }
        public DateTime Date { get; private set; }

        public UpdatedStockPriceMessage(decimal price, DateTime date)
        {
            Price = price;
            Date = date;
        }
    }
}