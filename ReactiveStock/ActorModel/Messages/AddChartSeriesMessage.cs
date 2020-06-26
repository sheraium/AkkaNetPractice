namespace ReactiveStock.ActorModel.Messages
{
    public class AddChartSeriesMessage
    {
        public string StockSymbol { get; private set; }

        public AddChartSeriesMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}