using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// リザルトシーケンス
/// </sammary>
///--------------------------------
public class SQ_ResultScenectrl : MonoBehaviour
{
	///--------------------------------
	// シーケンスモード
	///--------------------------------
	public enum MODE
	{
		NONE,

		START,

		RESULT_UI_CREATE,

		RESULT_OPEN_INIT,
		RESULT_OPEN_LOOP,

		RESULT_INPUT_WAIT_INIT,
		RESULT_INPUT_WAIT_LOOP,

		RESULT_CLOSE_INIT,
		RESULT_CLOSE_LOOP,

		GAMEEND_OPEN_INIT,
		GAMEEND_OPEN_LOOP,

		GAMEEND_INPUT_WAIT_INIT,
		GAMEEND_INPUT_WAIT_LOOP,

		GAMEEND_CLOSE_INIT,
		GAMEEND_CLOSE_LOOP,

		CLOSE	
	};
	public MODE m_Mode = MODE.NONE;

	///--------------------------------
	// メンバー
	///--------------------------------
	private ResultScore	m_ResultUICtrl;

	private GameendUICtrl	m_GameendUICtrl;

	private bool m_ClickLock = true;

	///--------------------------------
	// 初期化
	///--------------------------------
	void Awake()
	{
		ModeChange( MODE.START );
	}

	///--------------------------------
	// 解放
	///--------------------------------
	private void ReleaseAllObjectsAndCtrls()
	{
		if( m_ResultUICtrl != null )
		{
			Destroy( m_ResultUICtrl.gameObject );
			m_ResultUICtrl = null;
		}

		if( m_GameendUICtrl != null )
		{
			Destroy( m_GameendUICtrl.gameObject );
			m_GameendUICtrl = null;
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

				ReleaseAllObjectsAndCtrls();

				ModeChange( MODE.RESULT_UI_CREATE );

				break;

			case MODE.RESULT_UI_CREATE:

				add_Object = Instantiate( Resources.Load( "Result/ResultUIPrefab" ) as GameObject );
				m_ResultUICtrl = add_Object.GetComponent<ResultScore>();
				//	m_TitleUICtrl.Init();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );

				add_Object = Instantiate( Resources.Load( "Result/GameEndUIPrefab" ) as GameObject );
				m_GameendUICtrl = add_Object.GetComponent<GameendUICtrl>();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );
				m_GameendUICtrl.gameObject.SetActive( false );

				ModeChange( MODE.RESULT_OPEN_INIT );

				break;

			case MODE.RESULT_OPEN_INIT:

				StaticAccess.m_WorldCanvas.FadeOpenRq();

				ModeChange( MODE.RESULT_OPEN_LOOP );

				break;

			case MODE.RESULT_OPEN_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeIn() )
				{
					break;
				}

				ModeChange( MODE.RESULT_INPUT_WAIT_INIT );

				break;

			case MODE.RESULT_INPUT_WAIT_INIT:

				m_ClickLock = false;

				ModeChange( MODE.RESULT_INPUT_WAIT_LOOP );

				break;

			case MODE.RESULT_INPUT_WAIT_LOOP:

				if( Input.GetKeyDown( KeyCode.Space ) ||
					( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) )
				{
					m_ClickLock = true;
					ModeChange( MODE.RESULT_CLOSE_INIT );
					break;
				}

				break;

			case MODE.RESULT_CLOSE_INIT:

				StaticAccess.m_WorldCanvas.FadeCloseRq();

				ModeChange( MODE.RESULT_CLOSE_LOOP );

				break;

			case MODE.RESULT_CLOSE_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeOut() )
				{
					break;
				}

				m_ResultUICtrl.gameObject.SetActive( false );

				ModeChange( MODE.CLOSE );

				break;

			case MODE.GAMEEND_OPEN_INIT:

				m_GameendUICtrl.gameObject.SetActive( true );

				StaticAccess.m_WorldCanvas.FadeOpenRq();

				ModeChange( MODE.GAMEEND_OPEN_LOOP );

				break;

			case MODE.GAMEEND_OPEN_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeIn() )
				{
					break;
				}

				ModeChange( MODE.GAMEEND_INPUT_WAIT_INIT );

				break;

			case MODE.GAMEEND_INPUT_WAIT_INIT:

				m_ClickLock = false;

				ModeChange( MODE.GAMEEND_INPUT_WAIT_LOOP );

				break;

			case MODE.GAMEEND_INPUT_WAIT_LOOP:

				if( Input.GetKeyDown( KeyCode.Space ) ||
					( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) )
				{
					m_ClickLock = true;
					ModeChange( MODE.GAMEEND_CLOSE_INIT );
					break;
				}

				break;

			case MODE.GAMEEND_CLOSE_INIT:

				StaticAccess.m_WorldCanvas.FadeCloseRq();

				ModeChange( MODE.GAMEEND_CLOSE_LOOP );

				break;

			case MODE.GAMEEND_CLOSE_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeOut() )
				{
					break;
				}

				ModeChange( MODE.CLOSE );

				break;

			case MODE.CLOSE:

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

	///--------------------------------
	// シーンクローズ検知
	///--------------------------------
	public bool IsClose()
	{
		if ( m_Mode == MODE.CLOSE )
		{
			return true;
		}

		return false;
	}
}
