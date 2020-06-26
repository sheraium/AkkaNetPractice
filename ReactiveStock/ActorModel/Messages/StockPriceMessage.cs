using System;

namespace ReactiveStock.ActorModel.Messages
{
    public class StockPriceMessage
    {
        public string StockSymbol { get; }
        public decimal StockPrice { get; }
        public DateTime Date { get; }

        public StockPriceMessage(string stockSymbol, decimal stockPrice, DateTime date)
        {
            StockSymbol = stockSymbol;
            StockPrice = stockPrice;
            Date = date;
        }
    }
}