using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using OrganControlLib;
using RemoteOrganControl.Server.Hubs;

namespace RemoteOrganControl.Server
{
    public class OrganController
    {
        private readonly IHubContext<MidiHub> _hub;
        public readonly Player Player = new Player();
        public readonly Recorder Recorder = new Recorder();
        private List<Smf> _smfs = new List<Smf>();
        
        public OrganController(IHubContext<MidiHub> hub)
        {
            _hub = hub;

            // TODO: get this to actually work - this EventHandler is deprecated - must be a replacement
            this.Player.access.StateChanged += (sender, args) =>
            {
                Console.WriteLine($"Device state changed: {args.Port.Id} - {args.Port.Name}");
                var outputDevices = Player.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
                _hub.Clients.All.SendAsync("ReceiveOutputDeviceList",  outputDevices);
                var inputDevices = Recorder.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
                _hub.Clients.All.SendAsync("ReceiveInputDeviceList",  inputDevices);
            };
        }

        public void InitCallback()
        {
            this.Recorder.GetInput().MessageReceived += (obj, e) =>
            {
                _hub.Clients.All.SendAsync("ReceiveMidiInputMessage", $"Time{e.Timestamp} Start{e.Start} Len{e.Length} {BitConverter.ToString(e.Data.Take(e.Length).ToArray())}").Wait();
            };
        }

        public void AddSmf(string filename, Stream stream)
        {
            _smfs.Add(new Smf(filename, stream));
            // Send all Smf names to clients
            _hub.Clients.All.SendAsync("ReceiveSmfNames", _smfs.Select(s => s.Name));
        }

        public void PlayNamedSMF(string smfName)
        {
            var smf = _smfs.First(s => s.Name == smfName);
            if (smf != null)
                Player.PlaySmf(smf.GetMusic());
        }

        public void StopSmf()
        {
            Player.StopSmf();
        }

    }
}