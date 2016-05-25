using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// WorldSpaceキャンバス
/// </sammary>
///--------------------------------
public class WorldCanvasCtrl : MonoBehaviour {


	[SerializeField]
	private RectTransform m_UIRectTransform;

	[SerializeField]
	private FadeImageCtrl m_FadeImageCtrl;

	[SerializeField]
	private Camera m_SelfCamera;

	[SerializeField]
	private float DISTANCE_CAMERA = 2.0f;

	private Vector3 m_Reverse = new Vector3( 0.0f, 180.0f, 0.0f );

	///--------------------------------
	/// <sammary>
	/// 初期化
	/// </sammary>
	///--------------------------------
	void Awake()
	{
		StaticAccess.m_WorldCanvas = this;
	}

	void Update()
	{
		gameObject.transform.position = ( m_SelfCamera.transform.forward * DISTANCE_CAMERA ) + Vector3.up;
		gameObject.transform.LookAt( m_SelfCamera.transform );
		gameObject.transform.Rotate( m_Reverse );
	}
	

	///--------------------------------
	/// <sammary>
	/// UIを親に設定
	/// </sammary>
	///--------------------------------
	public void AddUI( RectTransform t )
	{
		t.SetParent( m_UIRectTransform, false );
	}

	///--------------------------------
	/// <sammary>
	/// フェードイン開始
	/// </sammary>
	///--------------------------------
	public void FadeOpenRq()
	{
		m_FadeImageCtrl.OpenRq();
	}

	///--------------------------------
	/// <sammary>
	/// フェードイン中か
	/// </sammary>
	///--------------------------------
	public bool IsFadeIn()
	{
		return m_FadeImageCtrl.IsOpen();
	}

	///--------------------------------
	/// <sammary>
	/// フェードアウト開始
	/// </sammary>
	///--------------------------------
	public void FadeCloseRq()
	{
		m_FadeImageCtrl.CloseRq();
	}

	///--------------------------------
	/// <sammary>
	/// フェードアウト中か
	/// </sammary>
	///--------------------------------
	public bool IsFadeOut()
	{
		return m_FadeImageCtrl.IsClose();
	}

}
