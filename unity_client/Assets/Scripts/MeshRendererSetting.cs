using UnityEngine;
using System.Collections;

public class MeshRendererSetting : MonoBehaviour {

    [SerializeField]
    string layerName = "UI";

	// Use this for initialization
	void Start () {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        if (rend != null)  
        {
            rend.sortingLayerName = layerName;
        }
    }

}
