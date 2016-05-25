using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// タイトルシーケンス
/// </sammary>
///--------------------------------
public class SQ_TitleSceneCtrl : MonoBehaviour
{
	
	///--------------------------------
	// シーケンスモード
	///--------------------------------
	public enum MODE
	{
		NONE,

		START,
		
		TITLE_UI_CREATE,
		
		TITLE_OPEN_INIT,
		TITLE_OPEN_LOOP,

		INPUT_WAIT_INIT,
		INPUT_WAIT_LOOP,
		
		TITLE_CLOSE_INIT,
		TITLE_CLOSE_LOOP,
		
		CLOSE	
	};
	public MODE m_Mode = MODE.NONE;
	
	///--------------------------------
	// メンバー
	///--------------------------------
	private TitleUICtrl m_TitleUICtrl;

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
		if( m_TitleUICtrl != null )
		{
			Destroy( m_TitleUICtrl.gameObject );
			m_TitleUICtrl = null;
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

			ModeChange( MODE.TITLE_UI_CREATE );

			break;

			case MODE.TITLE_UI_CREATE:
			
				add_Object = Instantiate( Resources.Load( "Title/TitleUIPrefab" ) as GameObject );
				m_TitleUICtrl = add_Object.GetComponent<TitleUICtrl>();
			//	m_TitleUICtrl.Init();
				
				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );
				
				ModeChange( MODE.TITLE_OPEN_INIT );
				
				break;
				
			case MODE.TITLE_OPEN_INIT:

				StaticAccess.m_WorldCanvas.FadeOpenRq();

				m_TitleUICtrl.OpenRq();
				
				ModeChange( MODE.TITLE_OPEN_LOOP );
				
				break;
				
			case MODE.TITLE_OPEN_LOOP:
			
				if( !StaticAccess.m_WorldCanvas.IsFadeIn() )
				{
					break;
				}

				if( !m_TitleUICtrl.IsOpen() )
				{
					break;
				}

				ModeChange( MODE.INPUT_WAIT_INIT );
				
				break;

			case MODE.INPUT_WAIT_INIT:

				m_ClickLock = false;

				ModeChange( MODE.INPUT_WAIT_LOOP );

				break;

			case MODE.INPUT_WAIT_LOOP:

				if( Input.GetKeyDown( KeyCode.Space ) ||
					( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) )
				{
					m_ClickLock = true;
					ModeChange( MODE.TITLE_CLOSE_INIT );
					break;
				}

				break;

			case MODE.TITLE_CLOSE_INIT:

				StaticAccess.m_WorldCanvas.FadeCloseRq();

				ModeChange( MODE.TITLE_CLOSE_LOOP );

				break;

			case MODE.TITLE_CLOSE_LOOP:

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
