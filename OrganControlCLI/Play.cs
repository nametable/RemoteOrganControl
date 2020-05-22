using McMaster.Extensions.CommandLineUtils;
using OrganControlLib;

namespace OrganControlCLI
{
    [Command(Name = "play", Description = "Play midi data to device")]
    public class Play
    {
        // [Option]
        // public string Filename { get; set; }
        [Argument(0)]
        public string Filename { get; set; }
        [Option]
        private bool Test { get; set; }
        
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (this.Test)
            {
                var player = new Player();
                player.PlayTest();
            }
            console.WriteLine($"Filename: {Filename}");
            return 0;
        }
    }
}