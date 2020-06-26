using System;
using Akka.Actor;
using Akka.Dispatch;
using ConsoleApp1.Messages;

namespace ConsoleApp1.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");
            Receive<PlayMovieMessage>(message =>
            {
                Console.WriteLine("Received movie title " + message.MovieTitle);
                Console.WriteLine("Received user Id " + message.UserId);
            }, message => message.UserId == 42);
        }

        protected override void PreStart()
        {
            Console.WriteLine("PlaybackActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("PlaybackActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("PlaybackActor PreRestart because: " + reason);
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine("PlaybackActor PostRestart because: " + reason);
            base.PostRestart(reason);
        }
    }
}