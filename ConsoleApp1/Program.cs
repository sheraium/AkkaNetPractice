using System;
using Akka.Actor;

namespace ConsoleApp1
{
    internal class Program
    {
        public static ActorSystem MyActorSystem;

        private static void Main(string[] args)
        {
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            PrintInstructions();

            // make our first actors!
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()),
                "consoleWriterActor");
            IActorRef consoleReaderActor =
                MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)),
                    "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell("start");

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }

    internal class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var read = Console.ReadLine();
            if (!string.IsNullOrEmpty(read) && String.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the system (acquire handle to system via
                // this actors context)
                Context.System.Terminate();
                return;
            }

            // send input to the console writer to process and print
            _consoleWriterActor.Tell(read);

            // continue reading messages from the console
            Self.Tell("continue");
        }
    }

    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    internal class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var msg = message as string;

            // make sure we got a message
            if (string.IsNullOrEmpty(msg))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Please provide an input.\n");
                Console.ResetColor();
                return;
            }

            // if message has even # characters, display in red; else, green
            var even = msg.Length % 2 == 0;
            var color = even ? ConsoleColor.Red : ConsoleColor.Green;
            var alert = even ? "Your string had an even # of characters.\n" : "Your string had an odd # of characters.\n";
            Console.ForegroundColor = color;
            Console.WriteLine(alert);
            Console.ResetColor();
        }
    }
}