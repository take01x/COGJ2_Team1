using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///--------------------------------
/// <sammary>
/// UI表示メインキャンバス
/// </sammary>
///--------------------------------
public class MainCanvasCtrl : MonoBehaviour
{
	
	[SerializeField]
	private RectTransform m_UIRectTransform;

	[SerializeField]
	private FadeImageCtrl m_FadeImageCtrl;

	///--------------------------------
	/// <sammary>
	/// 初期化
	/// </sammary>
	///--------------------------------
	void Awake()
	{
	//	StaticAccess.m_MainCanvas = this;
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
