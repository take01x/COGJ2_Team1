using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// 判定テキストUI
/// </sammary>
///--------------------------------
public class JudgeTextUICtrl : MonoBehaviour
{
	private Animator m_Animator;

	void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

	///--------------------------------
	// 解放
	///--------------------------------
	private void ReleaseAllObjectsAndCtrls()
	{	
		System.GC.Collect ();

		Resources.UnloadUnusedAssets ();
	}

	///--------------------------------
	// 破棄
	///--------------------------------
	void OnDestroy()
	{
		ReleaseAllObjectsAndCtrls ();
	}

	public void PlayGood()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Good" );
	}

	public void PlayGreat()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Great" );
	}

	public void PlayParfect()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Parfect" );
	}

	public void PlayMiss()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Miss" );
	}

}
