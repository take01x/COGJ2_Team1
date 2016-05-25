using UnityEngine;
using System.Collections;

public class MeshRendererSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        if (rend)
        {
            rend.sortingLayerName = "UI";
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
