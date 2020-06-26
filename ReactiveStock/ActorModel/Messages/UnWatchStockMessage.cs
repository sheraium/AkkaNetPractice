namespace ReactiveStock.ActorModel.Messages
{
    public class UnWatchStockMessage
    {
        public string StockSymbol { get; private set; }

        public UnWatchStockMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}