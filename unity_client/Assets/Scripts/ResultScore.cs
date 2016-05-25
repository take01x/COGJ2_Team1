using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResultScore : MonoBehaviour {

	[SerializeField]
	int score = 8493;

	// 数字スプライト
	[SerializeField]
	private Sprite[] m_NumberSprites = new Sprite[10];

	[SerializeField]
	private Image[] m_NumberImages = new Image[6];

	// Use this for initialization
	void Start () {
		// for debug
		Debug.Log (score);
		SetScore( StaticAccess.m_Score );
	}

	public void SetScore(int _score) {
		string str = _score.ToString("D6");
		int index = 0;

		Debug.Log (str);

		foreach (char c in str) {
			int i = int.Parse (c.ToString());
			m_NumberImages[ index ].sprite = m_NumberSprites[ i ];
			index++;
		}
	}
}