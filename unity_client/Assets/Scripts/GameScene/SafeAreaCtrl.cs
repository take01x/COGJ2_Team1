using UnityEngine;
using System.Collections;

public class SafeAreaCtrl : MonoBehaviour {
	[SerializeField]
	private GameObject SM_SafeArea;

	[SerializeField]
	private GameObject FX_SafeArea;

	private Vector3 EfectPosition;

	float time = 0;

	[SerializeField]
	private bool PlayFlag;

	Camera maincamera;

	public void PlayEffect()
	{
		var _sm = Instantiate( this.SM_SafeArea, EfectPosition, Quaternion.identity ) as GameObject;
	}

	// Use this for initialization
	void Start () {
		this.maincamera = Camera.main;
		this.EfectPosition = maincamera.transform.position;
		this.EfectPosition.y *= 0.1f;
		this.PlayEffect ();
		this.PlayLoop ();
	}

	void PlayLoop() {
		var _fx = Instantiate( this.FX_SafeArea, EfectPosition, FX_SafeArea.transform.rotation ) as GameObject;
		StartCoroutine( "DeleteEffect", _fx );
	}
		
	IEnumerator DeleteEffect( GameObject _effect )
	{
		yield return new WaitForSeconds( 2 );

		Destroy (_effect);
	}
}
