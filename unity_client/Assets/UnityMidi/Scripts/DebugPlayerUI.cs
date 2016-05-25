using UnityEngine;
using UnityMidi;
using UnityEngine.UI;
using AudioSynthesis.Sequencer;
using System.Collections;

public class DebugPlayerUI : MonoBehaviour {

    [SerializeField]
    Text timerText;

    [SerializeField]
    Text judgedText;

    float count = 0;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () {
	    if(timerText != null && MidiEventSequencer.getInstance != null)
        {
            var time = MidiEventSequencer.getInstance.CurrentTime;
            timerText.text = (time/1000.0f).ToString();
        }

        if (judgedText.text != "")
        {
            count += Time.deltaTime;
            if (count >= 0.5)
            {
                judgedText.text = "";

            }
        }
	}

    public void OnFinished()
    {
        judgedText.text = "Finished!";
    }

    public void OnJudgedPerfect()
    {
        judgedText.text = "Perfect";
        count = 0;
    }
    public void OnJudgedGreat()
    {
        judgedText.text = "Great";
        count = 0;
    }
    public void OnJudgedGood()
    {
        judgedText.text = "Good";
        count = 0;
    }
    public void OnJudgedMiss()
    { 
        judgedText.text = "Miss";
        count = 0;
    }
}
