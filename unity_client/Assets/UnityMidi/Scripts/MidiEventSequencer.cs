/*
 *    ______   __ __     _____             __  __  
 *   / ____/__/ // /_   / ___/__  ______  / /_/ /_ 
 *  / /    /_  _  __/   \__ \/ / / / __ \/ __/ __ \
 * / /___ /_  _  __/   ___/ / /_/ / / / / /_/ / / /
 * \____/  /_//_/     /____/\__, /_/ /_/\__/_/ /_/ 
 *                         /____/                  
 * Midi File Sequencer 
 *  Used for situations where the whole midi is available in file format locally or over a network stream.
 *  Loads the midi and calculates the timing before hand so when sequencing no BPM calculation is needed.
 */
using System;
using AudioSynthesis.Midi;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Midi.Event;
using System.Collections.Generic;

namespace AudioSynthesis.Sequencer
{
    public class MidiEventSequencer
    {

        static byte NoteOn  = 0x90;
        static byte NoteOff = 0x80;

        static public bool IsNoteOn(MidiMessage data)
        {
            return (data.command & 0xF0) == NoteOn;
        }

        static public bool IsNoteOff(MidiMessage data)
        {
            return (data.command & 0xF0) == NoteOff;
        }


        static byte NormalNote = 60;
        static byte LongNote   = 48;
        static byte HeadBan    = 72;

        static public bool IsNormalNote(MidiMessage data)
        {
            return data.data1 == NormalNote;    // C4
        }
        static public bool IsLongNote(MidiMessage data)
        {
            return data.data1 == LongNote;    // C3
        }
        static public bool IsHeadBan(MidiMessage data)
        {
            return data.data1 == HeadBan;    // C5
        }



        static MidiEventSequencer instance = null;
        static public MidiEventSequencer getInstance
        {
            get { return instance; }
        }


        //--Variables
        //        private Synthesizer synth;

        public MidiMessage[] mdata;

        public List<MidiMessage> noteOnDataList = new List<MidiMessage>();

//        private bool[] blockList;       // チャンネルのミュート機能用らしい、ほうち

        private bool playing = false;
        private bool pausing = false;

//        private double playbackrate = 1.0; // 1/8 to 8
        private double totalTime;
        private double sampleTime;

        private double oldTime;

        private int eventIndex;

        public bool finished = false;



//        public Synthesizer Synth
//        {
//            get { return synth; }
//            set { synth = value; }
//        }

//        public int Size
//        {
//            get { return mdata.Length; }
//        }
        public int EventIndex
        {
            get { return eventIndex;  }
        }


        public bool IsPlaying
        {
            get { return playing; }
        }
        public bool IsPausing
        {
            get { return pausing; }
        }
        public bool IsMidiLoaded
        {
            get { return mdata != null; }
        }
        public double CurrentTime
        {
            get { return sampleTime; }
        }
        public double EndTime
        {
            get { return totalTime; }
        }
        public double GetDeltaTime()
        {
            return (sampleTime - oldTime);
        }

        public bool IsEventFinished
        {
            get { return finished; }
        }




        public MidiMessage GetCurrentMessage()
        {
            return noteOnDataList[eventIndex];//mdata[eventIndex];
        }

        public double GetNextMessageTime()
        {
            var tempIndex = eventIndex;
            if (tempIndex < noteOnDataList.Count - 1)
            {
                return noteOnDataList[tempIndex + 1].time;

            }
            // めんどうなので、少し大きい値でごまかす
            return noteOnDataList[noteOnDataList.Count - 1].time + 10000;
        }

        //--Public Methods
        public MidiEventSequencer()
        {
            instance = this;
//            this.synth = synth;
//            blockList = new bool[Synthesizer.DefaultChannelCount];
        }

        public bool LoadMidi(IResource midiFile)
        {
            if (playing == true)
                return false;
            LoadMidiFile(new MidiFile(midiFile));
            return true;
        }
        public bool LoadMidi(MidiFile midiFile)
        {
            if (playing == true)
                return false;
            LoadMidiFile(midiFile);
            return true;
        }
        public bool UnloadMidi()
        {
            if (playing == true)
                return false;
            mdata = null;
            return true;
        }
        public void Play()
        {
            if (playing == true || mdata == null)
                return;
            playing = true;
            eventIndex = 0;
            finished = false;
        }

        public void Pause()
        {
            if(playing)
            {
                pausing = true;
            }
        }

        public void Resume()
        {
            pausing = false;
        }

        public void Stop()
        {
            playing = false;
            sampleTime = 0;
            eventIndex = 0;
        }
        /*
        public bool IsChannelMuted(int channel)
        {
            return blockList[channel];
        }
        public void MuteAllChannels()
        {
            for (int x = 0; x < blockList.Length; x++)
                blockList[x] = true;
        }
        public void UnMuteAllChannels()
        {
            Array.Clear(blockList, 0, blockList.Length);
        }
        public void SetMute(int channel, bool muteValue)
        {
            blockList[channel] = muteValue;
        }
        */
        public bool NextNote()
        {
            if( eventIndex < noteOnDataList.Count - 1 )
            {
                ++eventIndex;
                return true;
            }
            // 終点ついてる
            finished = true;
            return false;
        }


        public void UpdateTime(double time)
        {
            oldTime = sampleTime;
            sampleTime = time;
        }

        public void UpdateDeltaTime(double deltaTime)
        {
            oldTime = sampleTime;
            sampleTime += deltaTime;
        }

//        public void Seek(TimeSpan time)
//        {
//            int targetSampleTime = (int)(synth.SampleRate * time.TotalSeconds);
//            if (targetSampleTime > sampleTime)
//            {//process forward
//                SilentProcess(targetSampleTime - sampleTime);
//            }
//            else if (targetSampleTime < sampleTime)
//            {//we have to restart the midi to make sure we get the right state: instruments, volume, pan, etc
//                sampleTime = 0;
//                eventIndex = 0;
//                synth.NoteOffAll(true);
//                synth.ResetPrograms();
//                synth.ResetSynthControls();
//                SilentProcess(targetSampleTime);
//            }
//        }
 //       public void FillMidiEventQueue()
 //       {
 //           if (!playing /*|| synth.midiEventQueue.Count != 0*/)
 //               return;
 //           if (sampleTime >= totalTime)
 //           {
 //               sampleTime = 0;
//                eventIndex = 0;
//                playing = false;
//                synth.NoteOffAll(true);
//                synth.ResetPrograms();
//                synth.ResetSynthControls();
//                return;
//            }
            
//            int newMSize = (int)(synth.MicroBufferSize * playbackrate);
//            for (int x = 0; x < synth.midiEventCounts.Length; x++)
//            {
//                sampleTime += newMSize;
//                while (eventIndex < mdata.Length && mdata[eventIndex].delta < sampleTime)
//                {
//                    if (mdata[eventIndex].command != 0x90 || blockList[mdata[eventIndex].channel] == false)
//                    {
//                        synth.midiEventQueue.Enqueue(mdata[eventIndex]);
//                        synth.midiEventCounts[x]++;
//                    }
//                    eventIndex++;
//                }
//            }
//        }
        //--Private Methods
        private void LoadMidiFile(MidiFile midiFile)
        {
            finished = false;

            noteOnDataList.Clear();

            //Converts midi to sample based format for easy sequencing
            double BPM = 120.0;
            //Combine all tracks into 1 track that is organized from lowest to highest absolute time
            if (midiFile.Tracks.Length > 1 || midiFile.Tracks[0].EndTime == 0)
                midiFile.CombineTracks();
            mdata = new MidiMessage[midiFile.Tracks[0].MidiEvents.Length];
            //Convert delta time to sample time
            eventIndex = 0;
            sampleTime = 0;
            //Calculate sample based time using double counter and round down to nearest integer sample.
            double absDelta = 0.0;
            for (int x = 0; x < mdata.Length; x++)
            {
                MidiEvent mEvent = midiFile.Tracks[0].MidiEvents[x];
                mdata[x] = new MidiMessage((byte)mEvent.Channel, (byte)mEvent.Command, (byte)mEvent.Data1, (byte)mEvent.Data2);
                //                absDelta += synth.SampleRate * mEvent.DeltaTime * (60.0 / (BPM * midiFile.Division));
                absDelta += mEvent.DeltaTime * (60.0f * 1000.0f / (BPM * midiFile.Division));
                mdata[x].time = absDelta;
                mdata[x].delta = (int)absDelta;
                //Update tempo
                if (mEvent.Command == 0xFF && mEvent.Data1 == 0x51)
                {
                    BPM = Math.Round(MidiHelper.MicroSecondsPerMinute / (double)((MetaNumberEvent)mEvent).Value, 2);
                }

                if ( IsNoteOn(mdata[x]) )
                {
                    var noteOn = mdata[x];

                    if (IsNormalNote(noteOn))
                    {
                        // ノートオンのみ収集しておく
                        noteOnDataList.Add(noteOn);
                    }
                    else {
                        if (IsLongNote(noteOn))
                        {
                            // ノートオンのみ収集しておく
                            noteOnDataList.Add(noteOn);
                        }
                        if(IsHeadBan(noteOn))
                        {
                            // ノートオンのみ収集しておく
                            noteOnDataList.Add(noteOn);
                        }

                        /*
                        if (x < mdata.Length - 1)
                        {
                            var noteOff = mdata[x + 1];
                            if ( IsLongNote(noteOn) && IsLongNote(noteOff) )
                            {
                                noteOn.length = noteOff.time - noteOn.time;
                                // ノートオンのみ収集しておく
                                noteOnDataList.Add(noteOn);

                            }
                            if (IsLongNote(noteOn) && IsLongNote(noteOff))
                            {
                                noteOn.length = noteOff.time - noteOn.time;
                                // ノートオンのみ収集しておく
                                noteOnDataList.Add(noteOn);
                            }
                        }
                        */
                    }
                }
            }
            //Set total time to proper value
            totalTime = mdata[mdata.Length - 1].time;
        }

        /*
        private MidiEvent searchNoteOff(double absDelta, int start, int end, int command, MidiEvent[] events)
        {
            for (int x = start; x <= end; x++)
            {
                MidiEvent mEvent = events[x];
                absDelta += mEvent.DeltaTime * (60.0f * 1000.0f / (BPM * midiFile.Division));
                if (events[x].Data1 == 0x80 && events[x].Command == command)
                {
                    return events[x];
                }
            }
            return null;
        }
        */


            //        private void SilentProcess(int amount)
            //        {
            //            while (eventIndex < mdata.Length && mdata[eventIndex].delta < (sampleTime + amount))
            //            {
            //                if (mdata[eventIndex].command != 0x90)
            //                {
            //                    MidiMessage m = mdata[eventIndex];
            //                    synth.ProcessMidiMessage(m.channel, m.command, m.data1, m.data2);
            //                }
            //                eventIndex++;
            //            }
            //            sampleTime += amount;
            //        }
        }
}
