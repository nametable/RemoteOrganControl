using System;
using McMaster.Extensions.CommandLineUtils;

namespace OrganControlCLI
{
    [Command(Name = "OrganControlCLI", Description = "CLI application for controlling organ(s)")]
    [Subcommand(typeof(Play))]
    class Program
    {
        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);
        
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            // Console.WriteLine("You must specify at a subcommand.");
            app.ShowHelp();
            return 1;
        }
    }
}
