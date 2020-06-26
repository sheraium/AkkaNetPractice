using System.Collections.Generic;
using Akka.Actor;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveStock.ActorModel.Messages;

namespace ReactiveStock.ActorModel.Actors
{
    public class LineChartingActor : ReceiveActor
    {
        private readonly PlotModel _chartModel;
        private readonly Dictionary<string, LineSeries> _series;

        public LineChartingActor(PlotModel chartModel)
        {
            _chartModel = chartModel;
            _series = new Dictionary<string, LineSeries>();

            Receive<AddChartSeriesMessage>(message => AddSeriesToChart(message));
            Receive<RemoveChartSeriesMessage>(message => RemoveSeriesFromChart(message));
            Receive<StockPriceMessage>(message => HandleNewStockPrice(message));
        }

        private void HandleNewStockPrice(StockPriceMessage message)
        {
            if (_series.ContainsKey(message.StockSymbol))
            {
                var series = _series[message.StockSymbol];
                var newDataPoint = new DataPoint(DateTimeAxis.ToDouble(message.Date),
                    Axis.ToDouble(message.StockPrice));
                if (series.Points.Count > 10)
                {
                    series.Points.RemoveAt(0);
                }

                series.Points.Add(newDataPoint);
                RefreshChart();
            }
        }

        private void RemoveSeriesFromChart(RemoveChartSeriesMessage message)
        {
            if (_series.ContainsKey(message.StockSymbol))
            {
                var seriesToRemove = _series[message.StockSymbol];
                _chartModel.Series.Remove(seriesToRemove);
                _series.Remove(message.StockSymbol);
                RefreshChart();
            }
        }

        private void RefreshChart()
        {
            _chartModel.InvalidatePlot(true);
        }

        private void AddSeriesToChart(AddChartSeriesMessage message)
        {
            if (!_series.ContainsKey(message.StockSymbol))
            {
                var newLineSeries = new LineSeries()
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Black,
                    MarkerType = MarkerType.None,
                    CanTrackerInterpolatePoints = false,
                    Title = message.StockSymbol,
                };
                _series.Add(message.StockSymbol, newLineSeries);
                _chartModel.Series.Add(newLineSeries);
                RefreshChart();
            }
        }
    }
}