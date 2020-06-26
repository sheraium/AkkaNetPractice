using System.Windows.Input;
using Akka.Actor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ActorModel.Messages;

namespace ReactiveStock.ViewModel
{
    public class StockToggleButtonViewModel : ViewModelBase
    {
        private string _buttonText;

        public string StockSymbol { get; set; }
        public ICommand ToggleCommand { get; set; }
        public IActorRef StockToggleButtonActorRef { get; private set; }

        public string ButtonText
        {
            get { return _buttonText; }
            set { Set(() => ButtonText, ref _buttonText, value); }
        }

        public StockToggleButtonViewModel(IActorRef stocksCoordinatorRef, string stockSymbol)
        {
            StockSymbol = stockSymbol;
            StockToggleButtonActorRef =
                ActorSystemReference.ActorSystem
                    .ActorOf(Props.Create(() =>
                        new StockToggleButtonActor(stocksCoordinatorRef, this, stockSymbol)));

            ToggleCommand = new RelayCommand(() => StockToggleButtonActorRef.Tell(new FlipToggleMessage()));
        }

        public void UpdateButtonTextToOff()
        {
            ButtonText = ConstructButtonText(false);
        }

        public void UpdateButtonTextToOn()
        {
            ButtonText = ConstructButtonText(true);
        }

        private string ConstructButtonText(bool isToggledOn)
        {
            return $"{StockSymbol} {(isToggledOn ? "(on)" : "(off)")}";
        }
    }
}