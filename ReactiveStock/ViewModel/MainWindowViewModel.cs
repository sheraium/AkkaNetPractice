using System.Collections.Generic;
using Akka.Actor;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;

namespace ReactiveStock.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IActorRef _chartingActorRef;
        private IActorRef _stocksCoordinatorActorRef;
        private PlotModel _plotModel;

        public Dictionary<string, StockToggleButtonViewModel> StockButtonViewModels { get; set; }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { Set(() => PlotModel, ref _plotModel, value); }
        }

        public MainWindowViewModel()
        {
            SetUpChartModel();
            InitializeActors();
            CreateStockButtonViewModels();
        }

        private void CreateStockButtonViewModels()
        {
            StockButtonViewModels = new Dictionary<string, StockToggleButtonViewModel>();
            CreateStockButtonViewModel("AAAA");
            CreateStockButtonViewModel("BBBB");
            CreateStockButtonViewModel("CCCC");
        }

        private void CreateStockButtonViewModel(string stockSymbol)
        {
            var newViewModel = new StockToggleButtonViewModel(_stocksCoordinatorActorRef, stockSymbol);
            StockButtonViewModels.Add(stockSymbol, newViewModel);
        }

        private void InitializeActors()
        {
            _chartingActorRef = ActorSystemReference.ActorSystem.ActorOf(Props.Create(
                () => new LineChartingActor(PlotModel)));

            _stocksCoordinatorActorRef = ActorSystemReference.ActorSystem.ActorOf(
                Props.Create(() => new StocksCoordinatorActor(_chartingActorRef)), "StocksCoordinator");
        }

        private void SetUpChartModel()
        {
            _plotModel = new PlotModel()
            {
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black,
            };

            var stockDateTimeAxis = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Date",
                StringFormat = "HH:mm:ss",
            };

            _plotModel.Axes.Add(stockDateTimeAxis);

            var stockPriceAxis = new LinearAxis()
            {
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Price"
            };
            _plotModel.Axes.Add(stockPriceAxis);
        }
    }
}