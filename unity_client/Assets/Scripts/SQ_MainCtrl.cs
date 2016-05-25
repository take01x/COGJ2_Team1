using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// ゲームメインシーケンス
/// </sammary>
///--------------------------------
public class SQ_MainCtrl : MonoBehaviour
{
	///--------------------------------
	// シーケンスモード
	///--------------------------------
	public enum MODE
	{
		NONE,
		
		START,
		
		TITLE_INIT,
		TITLE_LOOP,

		GAME_MAIN_INIT,
		GAME_MAIN_LOOP,

		RESULT_INIT,
		RESULT_LOOP,

		END	
	};
	public MODE m_Mode = MODE.NONE;
	
	///--------------------------------
	// メンバー
	///--------------------------------

	private SQ_TitleSceneCtrl	m_SQ_TitleSceneCtrl;

	private SQ_GameSceneCtrl	m_SQ_GameMainCtrl;

	private SQ_ResultScenectrl	m_SQ_ResultSceneCtrl;

	///--------------------------------
	// 初期化
	///--------------------------------
	void Awake()
	{
		m_Mode = MODE.START;
	}
	
	///--------------------------------
	// 解放
	///--------------------------------
	private void ReleaseAllObjectsAndCtrls()
	{
		if( m_SQ_TitleSceneCtrl != null )
		{
			Destroy( m_SQ_TitleSceneCtrl.gameObject );
			m_SQ_TitleSceneCtrl = null;
		}
		
		if( m_SQ_GameMainCtrl != null )
		{
			Destroy( m_SQ_GameMainCtrl.gameObject );
			m_SQ_GameMainCtrl = null;
		}

		if( m_SQ_ResultSceneCtrl != null )
		{
			Destroy( m_SQ_ResultSceneCtrl.gameObject );
			m_SQ_ResultSceneCtrl = null;
		}

		System.GC.Collect();
		
		Resources.UnloadUnusedAssets();
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
		GameObject add_Object;
		
		switch ( m_Mode )
		{
			case MODE.START:
				
				ModeChange( MODE.TITLE_INIT );
				break;
				
			case MODE.TITLE_INIT:
			
				add_Object = Instantiate( Resources.Load( "SQ_TitleSceneCtrlPrefab" ) as GameObject ) ;
				m_SQ_TitleSceneCtrl = add_Object.GetComponent<SQ_TitleSceneCtrl>();

				ModeChange( MODE.TITLE_LOOP );
				
				break;
			
			case MODE.TITLE_LOOP:

				if( !m_SQ_TitleSceneCtrl.IsClose() )
				{
					break;
				}

				ReleaseAllObjectsAndCtrls();
			
				ModeChange( MODE.GAME_MAIN_INIT );

				break;

			case MODE.GAME_MAIN_INIT:

				add_Object = Instantiate( Resources.Load( "SQ_GameSceneCtrlPrefab" ) as GameObject );
				m_SQ_GameMainCtrl = add_Object.GetComponent<SQ_GameSceneCtrl>();

				ModeChange( MODE.GAME_MAIN_LOOP );

				break;

			case MODE.GAME_MAIN_LOOP:

				if( !m_SQ_GameMainCtrl.IsClose() )
				{
					break;
				}

				ReleaseAllObjectsAndCtrls();

				ModeChange( MODE.RESULT_INIT );

				break;

			case MODE.RESULT_INIT:

				add_Object = Instantiate( Resources.Load( "SQ_ResultSceneCtrlPrefab" ) as GameObject );
				m_SQ_ResultSceneCtrl = add_Object.GetComponent<SQ_ResultScenectrl>();

				ModeChange( MODE.RESULT_LOOP );

				break;

			case MODE.RESULT_LOOP:

				if( !m_SQ_ResultSceneCtrl.IsClose() )
				{
					break;
				}

				ReleaseAllObjectsAndCtrls();

				ModeChange( MODE.TITLE_INIT );

				break;

		}
	}
	
	///--------------------------------
	// モード切り替え
	///--------------------------------
	private void ModeChange( MODE mode )
	{
		m_Mode = mode;
	}
}
