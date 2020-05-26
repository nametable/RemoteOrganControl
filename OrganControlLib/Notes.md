###Good info here
https://www.cs.cmu.edu/~music/cmsip/readings/MIDI%20tutorial%20for%20programmers.html
https://sites.uci.edu/camp2014/2014/04/30/managing-midi-pitchbend-messages/
http://www.homerecordinghub.com/midi-channels.html
https://www.midi.org/specifications-old/item/table-2-expanded-messages-list-status-bytes

MIDI Message
------
Bytes|Name|Description
---|---|---
1|STATUS|bit 7 set to 1
varies|DATA|bit 7 set to 0

`Last status byte is what counts when interpretting data - MIDI RUNNING STATUS`

 - The NOTE ON message is structured as follows:
```
Status byte : 1001 CCCC
Data byte 1 : 0PPP PPPP
Data byte 2 : 0VVV VVVV
```
where:
```
"CCCC" is the MIDI channel (from 0 to 15)
"PPP PPPP" is the pitch value (from 0 to 127)
"VVV VVVV" is the velocity value (from 0 to 127)
```

- The MIDI message used to specify the instrument is called a "program change" message. It has one STATUS byte and one DATA byte :
```
Status byte : 1100 CCCC
Data byte 1 : 0XXX XXXX
```
- Controller Message
```angular
Status byte : 1011 CCCC
Data byte 1 : 0NNN NNNN
Data byte 2 : 0VVV VVVV
```
where `CCCC` is the MIDI channel, `NNNNNNN` is the controller number (from 0 to 127) and `VVVVVVV` is the value assigned to the controller (also from 0 to 127).
- Pitch Bend
```
Status byte : 1110 CCCC
Data byte 1 : 0LLL LLLL
Data byte 2 : 0MMM MMMM
```
- Note reset (all notes in channel)
  - `Midi Controller 123`
  - `Midi Status  :0xFF`
  - `Nidu Note Off:0x9{channel} 0xPPVV`
  - `Keep a table of notes that are on`
- Midi Tracks
  - They are a software concept
  - Limited only by software
  - For example, one might create left and right tracks using the same channel for the left and right hands for Piano
  - Each track needs to play on a channel, but multiple tracks could use the same channel

Rodgers SysEx Messages
------
```
Status byte : 0b11110000 or 0xF0
Data byte 1 : 0bVVVVVVVV
Data bytes....
D
```
where V is for Vendor ID