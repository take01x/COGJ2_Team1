using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// ゲームスタートUI
/// </sammary>
///--------------------------------
public class GameStartUICtrl : MonoBehaviour
{
	private Animator m_Animator;

	private bool m_OpenRq	= false;

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
	}

	public void OpenRq()
	{
		m_OpenRq = true;
	}

	public bool IsOpen()
	{
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

		if( stateInfo.nameHash ==  Animator.StringToHash( "Base Layer.Open" ) )
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
}
