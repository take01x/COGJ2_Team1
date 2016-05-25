using UnityEngine;
using System.Collections;

public class line : MonoBehaviour {
	LineRenderer renderer;
	float deg = 0;
	float length = 3.0f;

	// Use this for initialization
	void Start () {
		renderer = gameObject.GetComponent<LineRenderer>();
		// 線の幅
		renderer.SetWidth(0.1f, 0.1f);
		// 頂点の数
		renderer.SetVertexCount(2);
		// 頂点を設定
		renderer.SetPosition(0, new Vector3(0f, 0f, 1f));
		renderer.SetPosition(1, new Vector3(0f, 5f, 1f));
	}
	
	// Update is called once per frame
	void Update () {
		deg += 0.2f;
		float x = length * Mathf.Sin (deg * Mathf.Deg2Rad);
		float z = length * Mathf.Cos (deg * Mathf.Deg2Rad);
		renderer.SetPosition(0, new Vector3(x, 0f, z));
		renderer.SetPosition(1, new Vector3(x, 5f, z));
	}
}
