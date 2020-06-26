using System;
using Akka.Actor;
using ConsoleApp1.Actors;
using ConsoleApp1.Messages;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor system Created");

            //var playbackActorProps = Props.Create<PlaybackActor>();
            //IActorRef playbackActorRef = actorSystem.ActorOf(playbackActorProps, "PlaybackActor");
            var userActorProps = Props.Create<UserActor>();
            var userActorRef = actorSystem.ActorOf(userActorProps, "UserActor");

            userActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 42));

            userActorRef.Tell(new PlayMovieMessage("Boolean Lies", 42));

            userActorRef.Tell(new StopMovieMessage());

            userActorRef.Tell(new StopMovieMessage());

            Console.ReadLine();

            actorSystem.Dispose();
        }
    }
}