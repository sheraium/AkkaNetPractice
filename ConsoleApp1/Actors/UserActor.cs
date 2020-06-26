using System;
using Akka.Actor;
using ConsoleApp1.Messages;

namespace ConsoleApp1.Actors
{
    public class UserActor : ReceiveActor
    {
        private string _currentlyWatching;

        public UserActor()
        {
            Console.WriteLine("Creating a User Actor");
            Console.WriteLine("Setting initial behaviour to stopped");
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message =>
            {
                Console.WriteLine("Error: cannot start playing another movie before stopping existing one");
            });
            Receive<StopMovieMessage>(message =>
            {
                StopPlayingCurrentMovie();
            });
            Console.WriteLine("UserActor has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => { StartPlayerMovie(message.MovieTitle); });
            Receive<StopMovieMessage>(message =>
            {
                Console.WriteLine("Error: cannot stop if nothing is playing");
            });
            Console.WriteLine("UserActor has no become Stopped");
        }

        private void StopPlayingCurrentMovie()
        {
            Console.WriteLine($"User has stopped watching {_currentlyWatching}");
            _currentlyWatching = null;
            Become(Stopped);
        }

        private void StartPlayerMovie(string title)
        {
            _currentlyWatching = title;
            Console.WriteLine($"User is currently watching {_currentlyWatching}");
            Become(Playing);
        }

        protected override void PreStart()
        {
            Console.WriteLine("UserActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("UserActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("UserActor PreRestart because: " + reason);
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine("UserActor PostRestart because: " + reason);
            base.PostRestart(reason);
        }
    }
}