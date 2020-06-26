using System;

namespace ReactiveStock.ExternalServices
{
    public class RandomStockPriceServiceGateway : IStockPriceServiceGateway
    {
        private decimal _lastRandomPrice = 20;
        private readonly Random _random = new Random();

        public decimal GetLatestPrice(string stockSymbol)
        {
            var newPrice = _lastRandomPrice + _random.Next(-5, 5);
            if (newPrice < 0)
            {
                newPrice = 5;
            }
            else if (newPrice > 50)
            {
                newPrice = 45;
            }
            _lastRandomPrice = newPrice;
            return newPrice;
        }
    }
}