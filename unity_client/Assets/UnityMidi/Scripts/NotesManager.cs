using UnityEngine;
using UnityMidi;
using UnityEngine.UI;
using AudioSynthesis.Sequencer;
using System.Collections;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Midi;
using System.Collections.Generic;


public class NotesManager : MonoBehaviour {

    static NotesManager instance = null;

    static public NotesManager getInstance
    {
        get { return instance; }
    }

    public bool IsStartNote
    {
        get { return noteStart; }
    }

    NotesManager()
    {
        instance = this;
    }


    bool noteStart = false;


//    [SerializeField]
//    MidiEventPlayer player = null;

//    [SerializeField]
//    GameObject normalNote = null;


//    [SerializeField]
//    GameObject longNote = null;

    public string normalNotePath = "";
    public string longNotePath = "";
    public string headbanNotePath = "";

    GameObject normalNote = null;
    GameObject longNote   = null;
    GameObject headbanNote = null;


    [SerializeField]
    float speed = 10.0f;

    [SerializeField]
    bool heightOn = false;

    [SerializeField]
    int heightCenter = 100;

    [SerializeField]
    float heightDegDuration = 15.0f;


    [SerializeField]
    float offsetTheta = -90;

    [SerializeField]
    float period = 0.20f;

    float totalDeg = 0.0f;

//    [SerializeField]
//    public Vector3 timelineVec = Vector3.forward;

    [SerializeField]
    GameObject basePointer = null;

    Vector3 basePoint = Vector3.forward;

    float baseDistance = 0.0f;

    public void SetPointer(GameObject obj)
    {
        basePointer = obj;
        basePoint = basePointer.transform.position;

    }

    public void SetPointer(Vector3 vec)
    {
        basePoint = vec;
        baseDistance = Vector3.Distance(basePoint, new Vector3(0.0f, 1.0f, 0.0f));
    }


    public void StartNotes()
    {
        noteStart = true;
    }

    public void StopNotes()
    {
        noteStart = false;
    }
 
    void Awake()
    {
        instance = this;
    }

    public void  LoadNotesPrefab() {
        normalNote = Resources.Load(normalNotePath) as GameObject;
        longNote   = Resources.Load(longNotePath) as GameObject;
        headbanNote = Resources.Load(headbanNotePath) as GameObject;

    }

    // Update is called once per frame
    void Update () {
	    
	}


    public Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    public void CreateNotes(MidiEventSequencer sequencer)
    {
        // 回転量を計算
        totalDeg = period * 360.0f;

        var endTime = sequencer.EndTime;
        Debug.Log( "EndTime:" + endTime);

        // マーカー生成
        Debug.Log( "noteOn count:" + sequencer.noteOnDataList.Count.ToString());
        for( int i = 0; i < sequencer.noteOnDataList.Count; ++i )
        {
            InstantiateNote(sequencer.noteOnDataList[i],  endTime);
        }
    }

    public Vector3 hitPostion = Vector3.zero;


  
    private void InstantiateNote(MidiMessage message, double endTime)
    {
        //GameObject noteType = null;
        Vector3 pos = basePoint;

        GameObject prefab = normalNote;
        if (MidiEventSequencer.IsNormalNote(message))
        {
            prefab = normalNote;
        }
        else if(MidiEventSequencer.IsLongNote(message))
        {
            prefab = longNote;
        }
        else if (MidiEventSequencer.IsHeadBan(message))
        {
            prefab = headbanNote;
        }

        // 計算して奥に置く
        float timelinAngle = offsetTheta + totalDeg * (float)(message.time/endTime);
        float heightAngle = (heightOn) ? (message.data2 - heightCenter) * heightDegDuration : 0.0f;
        // 奥行き
        float radius = baseDistance + speed * (message.delta / 1000.0f);
        pos = GetPosition(timelinAngle, heightAngle, radius);


        var noteObj = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
        //noteObj.transform.SetParent(this.transform);
        //noteObj.transform.LookAt(Camera.main.transform);

        var cube = noteObj.GetComponent<cube>();
        cube.m_OnDeleteCallback = (note) =>
        {
            //NotesManager.getInstance.hitPostion = note.gameObject.transform.position;
            MidiEventPlayer.getInstance.OnTrigger(note.Message, note.gameObject);
            //Debug.Log(NotesManager.getInstance.hitPostion);
        };

        cube.setMessage(message);
        cube.setTargetPoint(GetPosition(timelinAngle, heightAngle, baseDistance));
        cube.setSpeed(speed);
    }
}
