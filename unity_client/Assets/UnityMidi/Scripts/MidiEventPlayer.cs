using UnityEngine;
using System.IO;
using System.Collections;
using AudioSynthesis;
using AudioSynthesis.Bank;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Midi;
using System.Collections.Generic;
using UnityMidi;



//    [RequireComponent(typeof(AudioSource))]
public class MidiEventPlayer : MonoBehaviour
{

    public enum JudgeType{
        Perfect = 0,
        Great   = 1,
        Good    = 2,
        Miss    = 3
    }



    [System.Serializable]
    public struct JudgeTiming {
        public float perfect;
        public float great;
        public float good;
    }

    [SerializeField]
    JudgeTiming judgeTiming;

//        [SerializeField]
//        StreamingAssetResouce bankSource;

    [SerializeField]
    public StreamingAssetResouce midiSource;

//    [SerializeField]
//    public AudioClip[]

    [SerializeField]
    bool loadOnAwake = false;

    [SerializeField]
    bool playOnAwake = false;

//    [SerializeField]
    public bool autoPlayMode = false;

//    [SerializeField]
//    NotesManager notesManager = null;

//    [SerializeField]
//    GameObject judgeEffect = null;

    [SerializeField]
    bool isAudioSourceTimer = true;

    [SerializeField]
    float timeOffset = 0.0f;


    public delegate void OnAddCombo();
    private OnAddCombo add_combo_callback;
    public void bindAddComboCallback(OnAddCombo _callback)
    {
        add_combo_callback += _callback;
    }

    public delegate void OnResetCombo();
    private OnResetCombo reset_combo_callback;
    public void bindResetCallback(OnResetCombo _callback)
    {
        reset_combo_callback += _callback;
    }

    public delegate void OnPerfect();
    private OnPerfect perfect_callback;
    public void bindPerfectCallback(OnPerfect _callback)
    {
        perfect_callback += _callback;
    }

    public delegate void OnGreat();
    private OnGreat great_callback;
    public void bindGreatCallback(OnGreat _callback)
    {
        great_callback += _callback;
    }

    public delegate void OnGood();
    private OnGood good_callback;
    public void bindGoodCallback(OnGood _callback)
    {
        good_callback += _callback;
    }

	public delegate void OnMiss();
	private OnMiss miss_callback;
	public void bindMissCallback(OnMiss _callback)
	{
		miss_callback += _callback;
	}

    DebugPlayerUI debugUi = null;

    bool start = false;

//    public NotesManager notesManaer = null;


    int combo = 0;

    //        [SerializeField]
    //        int channel = 1;
    //        [SerializeField]
    //        int sampleRate = 44100;
    //        [SerializeField]
    //        int bufferSize = 1024;
    //        PatchBank bank;
    MidiFile midi;
    //        Synthesizer synthesizer;
//        AudioSource audioSource;
    MidiEventSequencer sequencer;
//        int bufferHead;
//        float[] currentBuffer;

//        public AudioSource AudioSource { get { return audioSource; } }

    public MidiEventSequencer Sequencer { get { return sequencer; } }

    //        public PatchBank Bank { get { return bank; } }

    public MidiFile MidiFile { get { return midi; } }


    private AudioSource audioSource = null;


    static public MidiEventPlayer instance = null;
    static public MidiEventPlayer getInstance
    {
        get{ return instance; }
    }

    MidiEventPlayer()
    {
        // 念のため
        instance = this;
    }

    public void Awake()
    {
        instance = this;
        //            synthesizer = new Synthesizer(sampleRate, channel, bufferSize, 1);
        //            sequencer = new MidiFileSequencer(synthesizer);

        start = false;
        debugUi = GetComponent<DebugPlayerUI>();
        audioSource = GetComponent<AudioSource>();

        sequencer = new MidiEventSequencer();
//            audioSource = GetComponent<AudioSource>();

        if (loadOnAwake)
        {
            //                LoadBank(new PatchBank(bankSource));
            LoadMidi(new MidiFile(midiSource));
        }

        if (playOnAwake)
        {
            Play();
        }
    }

    //        public void LoadBank(PatchBank bank)
    //        {
    //            this.bank = bank;
    //            synthesizer.UnloadBank();
    //            synthesizer.LoadBank(bank);
    //        }

    private bool isLoading;

    public bool IsLoading
    {
        get { return isLoading; }
    }

    public void Load(int index)
    {
        if(!isLoading)
        {
            LoadMidi(new MidiFile(midiSource));
            isLoading = true;
        }
    }


    public void LoadMidi(MidiFile midi)
    {
        this.midi = midi;
        sequencer.Stop();
        sequencer.UnloadMidi();
        sequencer.LoadMidi(midi);
        NotesManager.getInstance.CreateNotes(sequencer);
    }

    public void Play()
    {
        combo = 0;
        isLoading = false;
        audioSource.Play();
        sequencer.Play();
        NotesManager.getInstance.StartNotes();
        start = true;
    }

    public bool IsFinished()
    {
        return !audioSource.isPlaying && sequencer.IsEventFinished;
    }

    void FixedUpdate()
    {
        // まだスタートしてない
        if (!start) return;

        // 完全に終了している
        if (IsFinished())
        {
            OnFinished();
            return;
        }

        // ノートを回す
        if (!sequencer.IsEventFinished) {

            var message = sequencer.GetCurrentMessage();

            // オートプレイ用
            if (autoPlayMode)
            {
                var timer = sequencer.CurrentTime;
                if (timer >= message.time)
                {
                    OnJudgedPerfect();
                    sequencer.NextNote();
                }
            }

            // タイミングチェック
            CheckTiming(message);
        }

        // タイムカウンタ
        if (isAudioSourceTimer)
        {
            if (audioSource.isPlaying)
            {
                sequencer.UpdateTime(audioSource.time * 1000.0f);
            }
        }
        else
        {
            sequencer.UpdateDeltaTime(Time.fixedDeltaTime * 1000.0f);
        }
    }

    void CheckTiming(MidiMessage message)
    {
        // headbanWarning判定
        if (MidiEventSequencer.IsHeadBan(message) && sequencer.CurrentTime < message.time - judgeTiming.good)
        {
            nextHeadBanWarning = true;
        }
        else
        {
            nextHeadBanWarning = false;
        }


        if (IsInput(message))
        {
            // 入力をとった
            if (Judge(message))
            {
                sequencer.NextNote();
            }
        }
        else if (sequencer.CurrentTime > sequencer.GetNextMessageTime() - judgeTiming.good)
        {
            // 次の音符にめりこんでる   
            OnJudgedMiss();
            sequencer.NextNote();
        }
        else if (sequencer.CurrentTime > message.time + judgeTiming.good)
        {
            // スルーした
            OnJudgedMiss();
            sequencer.NextNote();
        }
    }

    bool JudgeLong(MidiMessage message)
    {
        return false;
    }

    bool Judge(MidiMessage message)
    {
            
        var timer = sequencer.CurrentTime;
        var delta = (timer - message.time);
//           Debug.Log("Judged. " + timer);

        if (delta >= -judgeTiming.perfect && delta <= judgeTiming.perfect)
        {
            OnJudgedPerfect();
            return true;
        }
        else if (delta >= -judgeTiming.great && delta <= judgeTiming.great)
        {
            OnJudgedGreat();
            return true;

        }
        else if (delta >= -judgeTiming.good && delta <= judgeTiming.good)
        {
            OnJudgedGood();
            return true;
        }
        else
        {
            // 何もしない
        }
        return false;
    }

    void OnFinished()
    {
        Debug.Log("Finished.");
        if (debugUi != null && debugUi.enabled)
        {
            debugUi.OnFinished();
        }
        // 完全に止めておく
        start = false;
    }

    void OnJudgedMiss()
    {
        Debug.Log("Miss.");

        combo = 0;
        reset_combo_callback();
		miss_callback();

        if(debugUi != null && debugUi.enabled)
        {
            debugUi.OnJudgedMiss();
        }
    }

    void OnJudgedGood()
    {
        Debug.Log("Good.");

        combo++;
        add_combo_callback();
        good_callback();

        if (debugUi != null && debugUi.enabled)
        {
            debugUi.OnJudgedGood();
        }
    }

    void OnJudgedGreat()
    {
        Debug.Log("Great.");

        combo++;
        add_combo_callback();
        great_callback();

        if (debugUi != null && debugUi.enabled)
        {
            debugUi.OnJudgedGreat();
        }
    }

    void OnJudgedPerfect()
    {
        Debug.Log("Perfect.");

        combo++;
        add_combo_callback();
        perfect_callback();

        if (debugUi != null && debugUi.enabled)
        {
            debugUi.OnJudgedPerfect();
        }
    }

    private bool trigger = false;
    private Vector3 hitPos  = new Vector3(0, 1, 2);

    public Vector3 HitPos
    {
        get { return hitPos; }
    }

    public void OnTrigger( MidiMessage message, GameObject obj )
    {
        Debug.Log("Trigger: " + message.time);
        trigger = true;
        hitPos = obj.transform.position;

        CheckTiming(message);
    }

    private bool headBanging = false;

    public void ActiveHeadBanging(bool flag)
    {
        headBanging = flag;
    }

    private bool nextHeadBanWarning = false;

    public bool IsNextHeadBanWarning()
    {
        return nextHeadBanWarning;
    }

    //    public void SetTrigger()
    //    {
    //        trigger = true;
    //    }

    bool IsInput(MidiMessage message) 
    {
        if(MidiEventSequencer.IsHeadBan(message))
        {
            return headBanging;
        }

        // とりあえず仮

		if (Input.GetKeyDown(KeyCode.Space) || 
			( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) )
        {
            return true;
        }
       
        if(trigger)
        {
            trigger = false;
            return true;
        }
        return false;
    }

//        void OnAudioFilterRead(float[] data, int channel)
//        {
//            Debug.Assert(this.channel == channel);
//            int count = 0;
//            while (count < data.Length)
//            {
//                if (currentBuffer == null || bufferHead >= currentBuffer.Length)
//                {
//                    sequencer.FillMidiEventQueue();
//                    synthesizer.GetNext();
//                    currentBuffer = synthesizer.WorkingBuffer;
//                    bufferHead = 0;
//                }
//                var length = Mathf.Min(currentBuffer.Length - bufferHead, data.Length - count);
//                System.Array.Copy(currentBuffer, bufferHead, data, count, length);
//                bufferHead += length;
//                count += length;
//            }
//        }
}
