namespace ReactiveStock.ActorModel.Messages
{
    public class WatchStockMessage
    {
        public string StockSymbol { get; private set; }

        public WatchStockMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}