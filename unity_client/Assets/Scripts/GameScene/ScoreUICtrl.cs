using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///--------------------------------
/// <sammary>
/// スコアUI
/// </sammary>
///--------------------------------
public class ScoreUICtrl : MonoBehaviour
{
	// 数字イメージ
	[SerializeField]
	private Sprite[] m_NumberSprites = new Sprite[10];

	// スコアイメージ[桁数]
	[SerializeField]
	private Image[] m_NumberImages = new Image[6];


	private int m_CurrentScore = 0;

	public int m_NewScore = 999999;

	void Update()
	{
		if( m_NewScore != m_CurrentScore )
		{
			if( m_NewScore < m_CurrentScore )
			{
				m_CurrentScore -= ( int )( m_CurrentScore - m_NewScore ) / 10;
			}
			else if( m_NewScore > m_CurrentScore )
			{
				m_CurrentScore += ( int )( m_NewScore - m_CurrentScore ) / 10;
			}

			SetScore();
		}
	}

	///--------------------------------
	/// <sammary>
	/// スコア設定
	/// </sammary>
	///--------------------------------
	private void SetScore()
	{
		int score = m_CurrentScore;

		for( int i = m_NumberImages.Length-1; i >= 0; i-- )
		{
			int digit = ( int )Mathf.Pow( 10.0f, ( float )i );
			int num = score / digit;
			m_NumberImages[ i ].sprite = m_NumberSprites[ num ];

			score = score % digit;
		}
	}

}
