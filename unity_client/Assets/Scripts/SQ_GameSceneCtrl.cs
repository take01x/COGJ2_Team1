using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// ゲームシーンシーケンス
/// </sammary>
///--------------------------------
public class SQ_GameSceneCtrl : MonoBehaviour
{
	///--------------------------------
	// シーケンスモード
	///--------------------------------
	public enum MODE
	{
		NONE,

		START,

		RESOURCE_LOAD_INIT,
		RESOURCE_LOAD_LOOP,

		FADE_OPEN_INIT,
		FADE_OPEN_LOOP,

		READY_ANIM_INIT,
		READY_ANIM_LOOP,

		INPUT_WAIT_INIT,
		INPUT_WAIT_LOOP,

		FADE_CLOSE_INIT,
		FADE_CLOSE_LOOP,

		CLOSE	
	};
	public MODE m_Mode = MODE.NONE;

	///--------------------------------
	// メンバー
	///--------------------------------
		
	private GameStartUICtrl			m_GameStartUICtrl;

	private ScoreUICtrl				m_ScoreUICtrl;

	private ComboUICtrl				m_ComboUICtrl;

	private ScopeUICtrl 			m_ScopeUICtrl;

	private JudgeTextUICtrl			m_JudgeTextUICtrl;

	private HeadBangingTextUICtrl	m_HeadBangingTextUICtrl;

	private NoteEffectEmitter 	m_NoteEffectEmitter;

	private MidiEventPlayer m_midiPlayer;

    private NotesManager m_notesManager;

	private GameObject m_SafeArea;

//    private GameObject m_pointerObj;

	// ヘドバン取得用
	private Vector3 m_BeforeLookVector = new Vector3();

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
		if( m_GameStartUICtrl != null )
		{
			Destroy( m_GameStartUICtrl.gameObject );
			m_GameStartUICtrl = null;
		}

		if( m_ScoreUICtrl != null )
		{
			Destroy( m_ScoreUICtrl.gameObject );
			m_ScoreUICtrl = null;
		}

		if( m_ComboUICtrl != null )
		{
			Destroy( m_ComboUICtrl.gameObject );
			m_ComboUICtrl = null;
		}

		if( m_ScopeUICtrl != null )
		{
			Destroy( m_ScopeUICtrl.gameObject );
			m_ScopeUICtrl = null;
		}

		if( m_JudgeTextUICtrl != null )
		{
			Destroy( m_JudgeTextUICtrl.gameObject );
			m_JudgeTextUICtrl = null;
		}

		if( m_HeadBangingTextUICtrl != null )
		{
			Destroy( m_HeadBangingTextUICtrl.gameObject );
			m_HeadBangingTextUICtrl = null;
		}

		if( m_NoteEffectEmitter != null )
		{
			Destroy( m_NoteEffectEmitter.gameObject );
			m_NoteEffectEmitter = null;
		}

        if(m_notesManager != null)
        {
            Destroy( m_notesManager.gameObject);
            m_notesManager = null;
        }

        if(m_midiPlayer != null)
        {
            Destroy( m_midiPlayer.gameObject );
            m_midiPlayer = null;
        }

		if(m_SafeArea != null)
		{
			Destroy( m_SafeArea );
			m_SafeArea = null;
		}
        
//        if(m_pointerObj != null)
//        {
//            Destroy(m_pointerObj);
//            m_pointerObj = null;
//        }

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

	void LateUpdate()
	{
		GvrViewer.Instance.UpdateState();
		if (GvrViewer.Instance.BackButtonPressed) {
			Application.Quit();
		}
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

				ModeChange( MODE.RESOURCE_LOAD_INIT );

				break;

			case MODE.RESOURCE_LOAD_INIT:
			
                // プレイヤー
				add_Object = Instantiate( Resources.Load( "GameScene/Logic/MidiEventPlayer" ) as GameObject );

                m_midiPlayer = add_Object.GetComponent<MidiEventPlayer>();
                // debug
                //m_midiPlayer.autoPlayMode = true;

                // ノーツ管理
                add_Object = Instantiate(Resources.Load("GameScene/Logic/NotesManager") as GameObject);
                m_notesManager = add_Object.GetComponent<NotesManager>();
                m_notesManager.SetPointer(new Vector3(0.0f, 1.0f, 3.0f));
                m_notesManager.normalNotePath  = "GameScene/Note/SM_note_green";
                m_notesManager.longNotePath    = "GameScene/Note/SM_note_green";
                m_notesManager.headbanNotePath = "GameScene/Note/SM_note_headbanging";
                m_notesManager.LoadNotesPrefab();
                ////

				// ノートエフェクト生成
				add_Object = Instantiate( Resources.Load( "GameScene/UI/NoteEffectEmmiter" ) as GameObject );
				m_NoteEffectEmitter = add_Object.GetComponent<NoteEffectEmitter>();

				// スコアUI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/ScoreUIPrefab" ) as GameObject );
				m_ScoreUICtrl = add_Object.GetComponent<ScoreUICtrl>();
                m_ScoreUICtrl.m_NewScore = 0;


                StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );

				// コンボUI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/ComboUIPrefab" ) as GameObject );
				m_ComboUICtrl = add_Object.GetComponent<ComboUICtrl>();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );

				// ゲームスタートUI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/StartTextUIPrefab" ) as GameObject );
				m_GameStartUICtrl = add_Object.GetComponent<GameStartUICtrl>();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );

				// 判定UI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/JudgeTextPrefab" ) as GameObject );
				m_JudgeTextUICtrl = add_Object.GetComponent<JudgeTextUICtrl>();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );

				// ヘドバンUI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/HeadBangingUIPrefab" ) as GameObject );
				m_HeadBangingTextUICtrl = add_Object.GetComponent<HeadBangingTextUICtrl>();

				StaticAccess.m_WorldCanvas.AddUI( add_Object.GetComponent<RectTransform>() );
				m_HeadBangingTextUICtrl.gameObject.SetActive( false );

				// スコープUI
				add_Object = Instantiate( Resources.Load( "GameScene/UI/TX_scopering02" ) as GameObject );
				m_ScopeUICtrl = add_Object.GetComponent<ScopeUICtrl>();

				add_Object.transform.SetParent( Camera.main.transform, false );

				// SafeArea
				add_Object = Instantiate( Resources.Load( "GameScene/UI/SafeArea" ) as GameObject );
				m_SafeArea = add_Object.GetComponent<GameObject>();

                /// デリゲートで紐付け
                
                m_midiPlayer.bindAddComboCallback(
                    m_ComboUICtrl.AddCombo
                );

                m_midiPlayer.bindResetCallback(
                    m_ComboUICtrl.ResetCombo
                );

                m_midiPlayer.bindPerfectCallback(
                    () =>
                    {
					//	m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_PERFECT, m_midiPlayer.HitPos);
						m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_PERFECT, m_ScopeUICtrl.transform.position );
                        m_JudgeTextUICtrl.PlayParfect();
                        StaticAccess.m_SoundManager.PlaySE(SoundManager.SE_PERFECT);
                        m_ScoreUICtrl.m_NewScore = m_ScoreUICtrl.m_NewScore + 100;
                    }
                );
                m_midiPlayer.bindGreatCallback(
                    () =>
                    {
					//	m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_GREAT, m_midiPlayer.HitPos);
						m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_GREAT, m_ScopeUICtrl.transform.position );
                        m_JudgeTextUICtrl.PlayGreat();
                        StaticAccess.m_SoundManager.PlaySE(SoundManager.SE_GREAT);
                        m_ScoreUICtrl.m_NewScore = m_ScoreUICtrl.m_NewScore + 75;
                    }
                );
                m_midiPlayer.bindGoodCallback(
                    () =>
                    {
					//	m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_GOOD, m_midiPlayer.HitPos);
						m_NoteEffectEmitter.PlayEffect(NoteEffectEmitter.EFFECT_GOOD, m_ScopeUICtrl.transform.position );
                        m_JudgeTextUICtrl.PlayGood();
                        StaticAccess.m_SoundManager.PlaySE( SoundManager.SE_GOOD );
                        m_ScoreUICtrl.m_NewScore = m_ScoreUICtrl.m_NewScore + 10;
                    }
                );
				m_midiPlayer.bindMissCallback(
					() =>
					{
						m_JudgeTextUICtrl.PlayMiss();
					}
				);


                ModeChange( MODE.RESOURCE_LOAD_LOOP );

				break;

			case MODE.RESOURCE_LOAD_LOOP:

                // 1ループ回さないとうまくうごかなかった...
                if(!m_midiPlayer.IsLoading)
                {
                    // todo: 曲選択
                    m_midiPlayer.Load(0);
                }

                if ( m_ScoreUICtrl == null )
				{
					break;
				}

				if ( m_ComboUICtrl == null )
				{
					break;
				}

				if ( m_GameStartUICtrl == null )
				{
					break;
				}

				if ( m_ScopeUICtrl == null )
				{
					break;
				}

				if ( m_JudgeTextUICtrl == null )
				{
					break;
				}

				if ( m_NoteEffectEmitter == null )
				{
					break;
				}

				ModeChange( MODE.FADE_OPEN_INIT );

				break;

			case MODE.FADE_OPEN_INIT:
				
				StaticAccess.m_WorldCanvas.FadeOpenRq();

				ModeChange( MODE.FADE_OPEN_LOOP );

				break;

			case MODE.FADE_OPEN_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeIn() )
				{
					break;
				}

				ModeChange( MODE.READY_ANIM_INIT );

				break;

			case MODE.READY_ANIM_INIT:

				m_GameStartUICtrl.OpenRq();

				ModeChange( MODE.READY_ANIM_LOOP );

				break;

			case MODE.READY_ANIM_LOOP:

				if( m_GameStartUICtrl.IsOpen() )
				{
					break;
				}

				ModeChange( MODE.INPUT_WAIT_INIT );

				break;

			case MODE.INPUT_WAIT_INIT:

				m_ClickLock = false;

				m_BeforeLookVector = Camera.main.transform.forward;

                // DEBUG
                //StartCoroutine( "GameEndCoroutine" );

                m_midiPlayer.Play();

                ModeChange( MODE.INPUT_WAIT_LOOP );

				break;

			case MODE.INPUT_WAIT_LOOP:

				// DEBUG
			//	m_HeadBangingTextUICtrl.gameObject.SetActive( IsHeadBanging() );
				m_midiPlayer.ActiveHeadBanging( IsHeadBanging() );

                if (m_midiPlayer.IsFinished())
                {
                    ModeChange(MODE.FADE_CLOSE_INIT);
                }
                break;

			case MODE.FADE_CLOSE_INIT:

				StaticAccess.m_WorldCanvas.FadeCloseRq();

				ModeChange( MODE.FADE_CLOSE_LOOP );

				break;

			case MODE.FADE_CLOSE_LOOP:

				if( !StaticAccess.m_WorldCanvas.IsFadeOut() )
				{
					break;
				}

				StaticAccess.m_Score = m_ScoreUICtrl.m_NewScore;

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
        Debug.Log("ModeChange: " + m_Mode.ToString() + " -> " + mode.ToString());
		m_Mode = mode;
	}

	///--------------------------------
	// ヘッドバンキング取得
	///--------------------------------
	private bool IsHeadBanging()
	{
		Vector3 m_CurrentVector = Camera.main.transform.forward;
		float angle = Mathf.Abs( Vector3.Angle( m_CurrentVector, m_BeforeLookVector ) );

		m_BeforeLookVector = m_CurrentVector;

		if( angle > 5.0f )
		{
			return true;
		}

		return false;
	}

	///--------------------------------
	// デバッグ：シーン終了
	///--------------------------------
	private IEnumerator GameEndCoroutine()
	{
		yield return new WaitForSeconds( 15.0f );

		ModeChange( MODE.FADE_CLOSE_INIT );
	}

	///--------------------------------
	// ノートクリックコールバック
	///--------------------------------
	public void OnNoteClickCallback( cube a_ClickedCube )
	{
		// ノートクリック時処理
		// CubeCreater生成時に設定

		// 以下判定・演出処理
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
