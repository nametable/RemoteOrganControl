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
            var devices = _midiInterface._player.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});// .Select(i => i.Name);
            await Clients.All.SendAsync("ReceiveOutputDeviceList",  devices);
        }
        public async Task GetInputDevices()
        {
            var devices = _midiInterface._recorder.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
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
                    // Notify users of the device change
                    await Clients.All.SendAsync("ReceiveOutput", new DeviceDetails()
                    {
                        DeviceName = _midiInterface._player.GetOutput().Details.Name,
                        DeviceId = _midiInterface._player.GetOutput().Details.Id,
                    });
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
                    // Notify users of change
                    await Clients.All.SendAsync("ReceiveInput", new DeviceDetails()
                    {
                        DeviceName = _midiInterface._recorder.GetInput().Details.Name,
                        DeviceId = _midiInterface._recorder.GetInput().Details.Id,
                    });
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
            // Send back to current user
            await Clients.User(this.Context.UserIdentifier).SendAsync("ReceiveOutput", new DeviceDetails()
            {
                DeviceName = _midiInterface._player.GetOutput().Details.Name,
                DeviceId = _midiInterface._player.GetOutput().Details.Id,
            });
        }

        public async Task GetInput()
        {
            // Send back to current user
            await Clients.User(this.Context.UserIdentifier).SendAsync("ReceiveInput", new DeviceDetails()
            {
                DeviceName = _midiInterface._recorder.GetInput().Details.Name,
                DeviceId = _midiInterface._recorder.GetInput().Details.Id,
            });
        }
    }
}