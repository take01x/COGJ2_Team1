using UnityEngine;
using System;
using System.Collections;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Synthesis;


public class cube : MonoBehaviour, IGvrGazeResponder {

	Vector3 targetPoint = new Vector3(0, 0, 0);

    Vector3 dpxps = new Vector3(0, 0, 0);

	Vector3 initialPoint = new Vector3(0, 0, 0);

	private bool m_IsOnGazeEnter = false;

    private MidiMessage message;

    public MidiMessage Message {
        get{ return message; }
    } 


	float initialDistance = 0;

    [SerializeField]
	float speed = 1.0f;

    private float defaltY = 0.0f;

	public Action<cube> m_OnDeleteCallback;

	// Use this for initialization
	void Start () {

		MeshRenderer rend = GetComponent<MeshRenderer>();
        if(rend != null)
        {
            rend = GetComponentInChildren<MeshRenderer>();
        }
        if (rend)
        {
            rend.sortingLayerName = "UI";
        }
		this.initialPoint = transform.position;
        defaltY = transform.rotation.y;
        updateSpeed();

    }

	// Update is called once per frame
	void Update () {
        if (!NotesManager.getInstance.IsStartNote) return;
        // speed [m/sec]
        // transform.position += dpxps * (float)MidiEventSequencer.getInstance.GetDeltaTime() / 1000.0f;//Time.deltaTime;
        // 計算誤差できるだけ起きないようにしてみる
        float t = (float)(MidiEventSequencer.getInstance.CurrentTime / message.time);
        transform.position = Vector3.Lerp(initialPoint, targetPoint, t);
//        transform.LookAt(targetPoint);
        this.CheckDistance ();
	}

	void CheckDistance() {
		Vector3 dpx = transform.position - this.initialPoint;
		float distance = Mathf.Sqrt (Mathf.Pow (dpx.x, 2) + Mathf.Pow (dpx.y, 2) + Mathf.Pow (dpx.z, 2));
		if (distance >= this.initialDistance)
			this.Delete ();
	}

	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}

	public void Delete() {
		Destroy (this.gameObject);
	}
		
	public void setTargetPoint(Vector3 vec) {
		this.targetPoint = vec;
        this.transform.LookAt(targetPoint);
    }

    public void setSpeed(float _speed)
    {
        speed = _speed;
        updateSpeed();
    }

    public void setMessage(MidiMessage _message)
    {
        message = _message;
    }


	public void updateSpeed() {
//		this.speed = speed;
		Vector3 dpx = targetPoint - this.initialPoint;
		this.initialDistance = Mathf.Sqrt (Mathf.Pow (dpx.x, 2) + Mathf.Pow (dpx.y, 2) + Mathf.Pow (dpx.z, 2));
		float time = this.initialDistance / speed;
		this.dpxps = dpx / time;
	}

	#region IGvrGazeResponder implementation

	public void OnGazeEnter() {
		SetGazedAt(true);

		m_IsOnGazeEnter = true;
	}

	public void OnGazeExit() {
		SetGazedAt(false);

		m_IsOnGazeEnter = false;
	}

	public void OnGazeTrigger() {

		if( !m_IsOnGazeEnter )
			return;

        Delete();
        if ( m_OnDeleteCallback != null )
		{
			m_OnDeleteCallback( this );
		}
    }

    #endregion
}
