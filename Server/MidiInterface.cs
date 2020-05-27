using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using OrganControlLib;
using RemoteOrganControl.Server.Hubs;

namespace RemoteOrganControl.Server
{
    public class MidiInterface
    {
        private IHubContext<MidiHub> Hub;
        public readonly Player _player = new Player();
        public readonly Recorder _recorder = new Recorder();
        
        public MidiInterface(IHubContext<MidiHub> hub)
        {
            Hub = hub;

            this._player.access.StateChanged += (sender, args) =>
            {
                Console.WriteLine($"Device state changed: {args.Port.Id} - {args.Port.Name}");
                var outputDevices = _player.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
                Hub.Clients.All.SendAsync("ReceiveOutputDeviceList",  outputDevices);
                var inputDevices = _recorder.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
                Hub.Clients.All.SendAsync("ReceiveInputDeviceList",  inputDevices);
            };
        }

        public void InitCallback()
        {
            this._recorder.GetInput().MessageReceived += (obj, e) =>
            {
                Hub.Clients.All.SendAsync("ReceiveMidiInputMessage", $"Time{e.Timestamp} Start{e.Start} Len{e.Length} {BitConverter.ToString(e.Data.Take(e.Length).ToArray())}").Wait();
            };
        }
    }
}