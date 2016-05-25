using UnityEngine;
using System.Collections;

public class EndUICtrl : MonoBehaviour {

	[SerializeField]
	private GameObject effect;

	private Camera maincamera;

	[SerializeField]
	float distanceScale = 1.5f;

	public void PlayEffect(Vector3 position)
	{
		var _effect = Instantiate( this.effect, position, Quaternion.identity ) as GameObject;
		//var _effect = Instantiate( this.effect ) as GameObject;
		_effect.transform.parent = maincamera.transform;
		StartCoroutine( "DeleteEffect", effect );
	}
		
	IEnumerator DeleteEffect( GameObject _effect )
	{
		yield return new WaitForSeconds( 1.0f );

		Destroy (_effect);
	}

	// Use this for initialization
	void Start () {
		this.maincamera = Camera.main;
		this.PlayEffect (maincamera.transform.position + maincamera.transform.forward * this.distanceScale);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
