using System;
using Akka.Actor;

namespace Lesson1_1_ActorsAndActorSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var actorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterActor = actorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var consoleReaderActor = actorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));
           
            consoleReaderActor.Tell("start");

            actorSystem.WhenTerminated.Wait();

        }
    }
}