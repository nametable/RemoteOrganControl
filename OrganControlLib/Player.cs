using System;
using System.Linq;
using Commons.Music.Midi;

namespace OrganControlLib
{
    public class Player
    {
        private IMidiAccess access;
        private IMidiOutput output;
        
        public Player()
        {
            this.access = MidiAccessManager.Default;
            this.output = access.OpenOutputAsync(access.Outputs.Last().Id).Result;
        }

        public void Play()
        {
            
        }

        public void PlayTest()
        {
            output.Send(new byte [] {0xC0, GeneralMidi.Instruments.AcousticGrandPiano}, 0, 2, 0); // There are constant fields for each GM instrument
            output.Send(new byte [] {MidiEvent.NoteOn, 0x40, 0x70}, 0, 3, 0); // There are constant fields for each MIDI event
            output.Send(new byte [] {MidiEvent.NoteOff, 0x40, 0x70}, 0, 3, 0);
            output.Send(new byte [] {MidiEvent.Program, 0x30}, 0, 2, 0); // Strings Ensemble
            output.Send(new byte [] {0x90, 0x40, 0x70}, 0, 3, 0);
            output.Send(new byte [] {0x80, 0x40, 0x70}, 0, 3, 0);
            output.CloseAsync();
        }
    }
}
