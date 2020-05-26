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
        [Option]
        private bool ListDevices { get; set; }
        [Option]
        private string Output { get; set; }
        
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            var player = new Player();
            if (this.Output != null)
            {
                var devices = player.GetDevices();
                foreach (var device in devices)
                {
                    if (device.Id.ToLower().Contains(this.Output) || device.Name.ToLower().Contains(this.Output))
                    {
                        player.SetOutputPort(device);
                        console.WriteLine($"Using {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                        break;
                    }
                }
            }
            if (this.ListDevices)
            {
                var devices = player.GetDevices();
                foreach (var device in devices)
                {
                    console.WriteLine($"Device {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                }
            }
            else if (this.Test)
            {
                player.PlayTest();
            }
            else if (Filename == null)
            {
                app.ShowHelp();
            }
            console.WriteLine($"Filename: {Filename}");
            return 0;
        }
    }
}