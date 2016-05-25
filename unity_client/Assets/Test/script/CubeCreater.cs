using UnityEngine;
using System;
using System.Collections;

public class CubeCreater : MonoBehaviour
{
	public GameObject cubePrefab;
	int i;

	public Action<cube> m_OnCubeDeleteCallback;

	// Use this for initialization
	void Start () {
		i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		i++;
		if (i >= 100) {
			i = 0;
			CreateCube ();
		}
	}

	void CreateCube()
	{
		var Cube = ( GameObject )Instantiate(	cubePrefab,
												new Vector3((float)UnityEngine.Random.Range(-15.0f, 15.0f),
												(float)UnityEngine.Random.Range(3.0f, 15.0f),
												(float)UnityEngine.Random.Range(-15.0f, 15.0f)), new Quaternion(0, 0, 0, 0)
		);
		Cube.GetComponent<cube>().m_OnDeleteCallback = m_OnCubeDeleteCallback;
	}
}
