using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// サウンド管理
/// </sammary>
///--------------------------------
public class SoundManager : MonoBehaviour
{
	// SE
	public const int SE_GOOD	= 0;
	public const int SE_GREAT	= 1;
	public const int SE_PERFECT	= 2;
	public const int SE_COUNT	= 3;

	[SerializeField]
	private AudioClip[] m_SE = new AudioClip[SE_COUNT];

	// 8チャンネル
	[SerializeField]
	private AudioSource[] m_AudioSources = new AudioSource[8];


	// Use this for initialization
	void Awake()
	{
		StaticAccess.m_SoundManager = this;
	}

	///--------------------------------
	// SE再生
	///--------------------------------
	public void PlaySE( int idx )
	{
		if( idx < 0 || idx >= SE_COUNT )
		{
			return;
		}

		for( int i = 0; i < m_AudioSources.Length; i++ )
		{
			if( m_AudioSources[ i ].isPlaying )
			{
				continue;
			}

			m_AudioSources[ i ].clip = m_SE[ idx ];
			m_AudioSources[ i ].Play();
			return;
		}
	}
}
