using System.IO;
using System.Security.Permissions;
using Commons.Music.Midi;

namespace OrganControlLib
{
    public class Smf
    {
        private Stream _data;
        private string _name;

        public string Name
        {            
            get { return _name; }
            set { _name = value; }
        }

        public Stream Data
        {            
            get { return _data; }
        }
        
        public Smf(string name, Stream data)
        {
            this._name = name;
            this._data = data;
        }

        public MidiMusic GetMusic()
        {
            _data.Seek(0, SeekOrigin.Begin);
            return MidiMusic.Read(_data);
        }
    }
}