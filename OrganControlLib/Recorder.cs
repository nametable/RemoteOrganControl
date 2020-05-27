using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Commons.Music.Midi;

namespace OrganControlLib
{
    public class Recorder
    {
        private IMidiAccess2 access;
        private IMidiInput input;
        
        public Recorder()
        {
            this.access = (IMidiAccess2)MidiAccessManager.Default;
            this.input = access.OpenInputAsync(access.Inputs.Last().Id).Result;
        }

        ~Recorder()
        {
            input.CloseAsync();
        }
        public void SetInputPort(IMidiPortDetails port)
        {
            input.CloseAsync();
            this.input = access.OpenInputAsync(port.Id).Result;
        }
        public void Play()
        {
            
        }

        // public void PlayTest()
        // {
        //     Console.WriteLine($"Playing to {output.Details.Name}");
        //     output.Send(new byte [] {0xC0, GeneralMidi.Instruments.AcousticGrandPiano}, 0, 2, 0); // There are constant fields for each GM instrument
        //     output.Send(new byte [] {MidiEvent.NoteOn, 0x40, 0x70}, 0, 3, 0); // There are constant fields for each MIDI event
        //     Thread.Sleep(1000);
        //     // output.Send(new byte [] {MidiEvent.NoteOff, 0x40, 0x70}, 0, 3, 0);
        //     output.Send(new byte [] {MidiEvent.Program, 0x30}, 0, 2, 0); // Strings Ensemble
        //     output.Send(new byte [] {0x90, 0x40, 0x70}, 0, 3, 0);
        //     output.Send(new byte [] {0x80, 0x40, 0x70}, 0, 3, 0);
        //     //output.CloseAsync();
        //
        // }

        public IMidiInput GetInput()
        {
            return this.input;
        }
        public IEnumerable<IMidiPortDetails> GetDevices()
        {
            var devices = MidiAccessManager.Default.Inputs;
            return devices;
        }
    }
}