using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Commons.Music.Midi;

namespace OrganControlLib
{
    public class Player
    {
        //private IMidiAccess access;
        public IMidiAccess2 access;
        private IMidiOutput output;
        
        public Player()
        {
            this.access = (IMidiAccess2)MidiAccessManager.Default;
            this.output = access.OpenOutputAsync(access.Outputs.Last().Id).Result;
        }

        ~Player()
        {
            output.CloseAsync();
        }
        public async void SetOutputPort(IMidiPortDetails port)
        {
            await output.CloseAsync();
            // output.Dispose();
            this.output = await access.OpenOutputAsync(port.Id);
        }
        public void Play()
        {
            
        }

        public void PlayTest()
        {
            Console.WriteLine($"Playing to {output.Details.Name}");
            // output.Send(new byte [] {0xC0, GeneralMidi.Instruments.AcousticGrandPiano}, 0, 2, 0); // There are constant fields for each GM instrument
            output.Send(new byte [] {MidiEvent.NoteOn, 0x40, 0x70}, 0, 3, 0); // There are constant fields for each MIDI event
            Thread.Sleep(1000);
            output.Send(new byte [] {MidiEvent.NoteOff, 0x40, 0x70}, 0, 3, 0);
            // output.Send(new byte [] {MidiEvent.Program, 0x30}, 0, 2, 0); // Strings Ensemble
            // output.Send(new byte [] {0x90, 0x40, 0x70}, 0, 3, 0);
            // output.Send(new byte [] {0x80, 0x40, 0x70}, 0, 3, 0);
            //output.CloseAsync();

        }

        public void PlaySMF() //(Stream stream)
        {
            var midiFileStream = File.OpenRead("Data/001-praise-to-the-lord.mid");
            Console.WriteLine($"Playing MIDI Stream");
            var music = MidiMusic.Read(midiFileStream);
            var player = new MidiPlayer(music, this.output);
            player.Play();
        }
        public IEnumerable<IMidiPortDetails> GetDevices()
        {
            var devices = MidiAccessManager.Default.Outputs;
            return devices;
        }

        public IMidiOutput GetOutput()
        {
            return this.output;
        }
    }
}
