using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// Imageのアルファフェード制御
/// </sammary>
///--------------------------------
public class FadeImageCtrl : MonoBehaviour
{

	private Animator m_Animator;

	private bool m_OpenRq	= false;
	private bool m_CloseRq	= false;

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

	///--------------------------------
	// 更新
	///--------------------------------
	void Update()
	{
		if( m_OpenRq )
		{
			if( IsClose() )
			{
				Open();
			}
			m_OpenRq = false;
		}

		if( m_CloseRq )
		{
			if( IsOpen() )
			{
				Close();
			}
			m_CloseRq = false;
		}
	}

	public void OpenRq()
	{
		m_OpenRq = true;
	}

	public void CloseRq()
	{
		m_CloseRq = true;
	}

	public bool IsOpen()
	{
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

		if( stateInfo.nameHash ==  Animator.StringToHash( "Base Layer.OpenLoop" ) )
		{
			return true;
		}
		return false;
	}

	public bool IsClose()
	{
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

		if( stateInfo.nameHash ==  Animator.StringToHash( "Base Layer.CloseLoop" ) )
		{
			return true;
		}
		return false;
	}

	private void Open()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Open" );
	}

	private void Close()
	{
		if ( m_Animator.IsInTransition (0) )
		{
			return;
		}

		m_Animator.SetTrigger( "Close" );
	}
}
