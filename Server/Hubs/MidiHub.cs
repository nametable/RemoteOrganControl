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

        private OrganController _organController;

        public MidiHub(OrganController organController)
        {
            _organController = organController;
        }
        
        public async Task GetOutputDevices()
        {
            var devices = _organController.Player.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});// .Select(i => i.Name);
            await Clients.All.SendAsync("ReceiveOutputDeviceList",  devices);
        }
        public async Task GetInputDevices()
        {
            var devices = _organController.Recorder.GetDevices().Select(i => new DeviceDetails(){ DeviceName = i.Name, DeviceId = i.Id});
            await Clients.All.SendAsync("ReceiveInputDeviceList",  devices);
        }

        public async Task SetOutputDevice(string deviceString)
        {
            var devices = _organController.Player.GetDevices();
            foreach (var device in devices)
            {
                if (device.Id.ToLower().Contains(deviceString.ToLower()) || device.Name.ToLower().Contains(deviceString.ToLower()))
                {
                    _organController.Player.SetOutputPort(device);
                    Console.WriteLine($"Output: Using {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                    // Notify users of the device change
                    await Clients.All.SendAsync("ReceiveOutput", new DeviceDetails()
                    {
                        DeviceName = _organController.Player.GetOutput().Details.Name,
                        DeviceId = _organController.Player.GetOutput().Details.Id,
                    });
                    break;
                }
            }
        }
        
        public async Task SetInputDevice(string deviceString)
        {
            var devices = _organController.Recorder.GetDevices();
            foreach (var device in devices)
            {
                if (device.Id.ToLower().Contains(deviceString.ToLower()) || device.Name.ToLower().Contains(deviceString.ToLower()))
                {
                    _organController.Recorder.SetInputPort(device);
                    Console.WriteLine($"Input: Using {device.Id}: {device.Name} - {device.Manufacturer} - {device.Version}");
                    // Notify users of change
                    await Clients.All.SendAsync("ReceiveInput", new DeviceDetails()
                    {
                        DeviceName = _organController.Recorder.GetInput().Details.Name,
                        DeviceId = _organController.Recorder.GetInput().Details.Id,
                    });
                    _organController.InitCallback();
                    break;
                }
            }
        }

        public async Task MidiTest()
        {
            await Task.Run(() =>
            {
                _organController.Player.PlayTest();
            });
        }

        public async Task PlaySmf(string smfName)
        {
            await Task.Run(() =>
            {
                _organController.PlayNamedSMF(smfName);
            });
        }
        
        public async Task StopSmf()
        {
            await Task.Run(() =>
            {
                _organController.StopSmf();
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
                DeviceName = _organController.Player.GetOutput().Details.Name,
                DeviceId = _organController.Player.GetOutput().Details.Id,
            });
        }

        public async Task GetInput()
        {
            // Send back to current user
            await Clients.User(this.Context.UserIdentifier).SendAsync("ReceiveInput", new DeviceDetails()
            {
                DeviceName = _organController.Recorder.GetInput().Details.Name,
                DeviceId = _organController.Recorder.GetInput().Details.Id,
            });
        }
    }
}