using Akka.Actor;
using Akka.DI.Ninject;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ExternalServices;

namespace ReactiveStock.ActorModel
{
    internal static class ActorSystemReference
    {
        public static ActorSystem ActorSystem { get; private set; }

        static ActorSystemReference()
        {
            CreateActorSystem();
        }

        private static void CreateActorSystem()
        {
            ActorSystem = ActorSystem.Create("ReactiveStockActorSystem");
            var container = new StandardKernel();
            container.Bind<IStockPriceServiceGateway>().To<RandomStockPriceServiceGateway>();
            container.Bind<StockPriceLookupActor>().ToSelf();
            var resolver = new NinjectDependencyResolver(container, ActorSystem);
        }
    }
}