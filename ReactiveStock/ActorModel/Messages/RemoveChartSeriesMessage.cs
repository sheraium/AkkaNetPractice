namespace ReactiveStock.ActorModel.Messages
{
    public class RemoveChartSeriesMessage
    {
        public string StockSymbol { get; private set; }

        public RemoveChartSeriesMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}