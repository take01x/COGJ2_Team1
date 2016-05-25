using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///--------------------------------
/// <sammary>
/// コンボUI
/// </sammary>
///--------------------------------
public class ComboUICtrl : MonoBehaviour
{
	// 数字オブジェクト[桁数]
	[SerializeField]
	private Image[] m_NumberImages = new Image[3];

	private Animator[] m_Animators = new Animator[3];

	// 数字スプライト
	[SerializeField]
	private Sprite[] m_NumberSprites = new Sprite[10];


	private int m_CurrentCombo = 0;

	void Awake()
	{
		for( int i = 0; i < m_NumberImages.Length; i++ )
		{
			m_Animators[ i ] = m_NumberImages[ i ].GetComponentInParent<Animator>();
			m_NumberImages[ i ].gameObject.SetActive( false ); 
		}

		gameObject.SetActive( false );
	}

	public void AddCombo()
	{
		if( !gameObject.activeSelf )
		{
			gameObject.SetActive( true );
		}
		m_CurrentCombo++;

		int combo = m_CurrentCombo;

		for( int i = m_NumberImages.Length-1; i >= 0; i-- )
		{
			int digit = ( int )Mathf.Pow( 10.0f, ( float )i );
			int num = combo / digit;

			if( num == 0 && m_CurrentCombo / digit * 10 == 0 )
			{
				m_NumberImages[ i ].gameObject.SetActive( false );
				continue;
			}

			m_NumberImages[ i ].gameObject.SetActive( true );
			m_NumberImages[ i ].sprite = m_NumberSprites[ num ];

			if ( !m_Animators[ i ].IsInTransition( 0 ) )
			{
				m_Animators[ i ].SetTrigger( "Open" );
			}

			combo = combo % digit;
		}
	}

	public void ResetCombo()
	{
		m_CurrentCombo = 0;

		for( int i = 0; i < m_NumberImages.Length; i++ )
		{
			m_NumberImages[ i ].gameObject.SetActive( false );
		}
		gameObject.SetActive( false );
	}

}
