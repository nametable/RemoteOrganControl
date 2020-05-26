using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Music.Midi;
using Microsoft.AspNetCore.SignalR;
using OrganControlLib;

namespace RemoteOrganControl.Server.Hubs
{
    public class MidiHub : Hub
    {

        private MidiInterface _midiInterface;

        public MidiHub(MidiInterface midiInterface)
        {
            _midiInterface = midiInterface;
        }
        
        public async Task GetOutputDevices()
        {
            var devices = _midiInterface._player.GetDevices().Select(i => i.Name);
            await Clients.All.SendAsync("ReceiveOutputDeviceList",  devices);
        }
        public async Task GetInputDevices()
        {
            var devices = _midiInterface._recorder.GetDevices().Select(i => i.Name);
            await Clients.All.SendAsync("ReceiveInputDeviceList",  devices);
        }

        public async Task SetOutputDevice(string deviceString)
        {
            var devices = _midiInterface._player.GetDevices();
            foreach (var device in devices)
            {
                if (device.Id.ToLower().Contains(deviceString.ToLower()) || device.Name.ToLower().Contains(deviceString.ToLower()))
                {
                    _midiInterface._player.SetOutputPort(device);
                    Console.WriteLine($"Output: Using {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                    break;
                }
            }
        }
        
        public async Task SetInputDevice(string deviceString)
        {
            var devices = _midiInterface._recorder.GetDevices();
            foreach (var device in devices)
            {
                if (device.Id.ToLower().Contains(deviceString.ToLower()) || device.Name.ToLower().Contains(deviceString.ToLower()))
                {
                    _midiInterface._recorder.SetInputPort(device);
                    Console.WriteLine($"Input: Using {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                    _midiInterface.InitCallback();
                    break;
                }
            }
        }

        public async Task MidiTest()
        {
            await Task.Run(() =>
            {
                _midiInterface._player.PlayTest();
            });
        }

        public async Task PlaySMF()
        {
            await Task.Run(() =>
            {
                _midiInterface._player.PlaySMF();
            });
        }
        
        public async Task SendMidiMessage(string msg)
        {
            Console.WriteLine("Should send msg");
            await Clients.All.SendAsync("ReceiveMidiInputMessage", msg);
        }

        public async Task SendMessage(string user, string message)
        {

        }

        public async Task GetOutput()
        {
            await Clients.All.SendAsync("ReceiveOutput",  _midiInterface._player.GetOutput().Details.Name);
        }

        public async Task GetInput()
        {
            await Clients.All.SendAsync("ReceiveInput",  _midiInterface._recorder.GetInput().Details.Name);
        }
    }
}