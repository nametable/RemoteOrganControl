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