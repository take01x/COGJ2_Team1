using UnityEngine;
using System.Collections;

///--------------------------------
/// <sammary>
/// ノートエフェクト生成
/// </sammary>
///--------------------------------
public class NoteEffectEmitter : MonoBehaviour
{
	public const int EFFECT_GOOD	= 0;
	public const int EFFECT_GREAT	= 1;
	public const int EFFECT_PERFECT	= 2;
	public const int EFFECT_COUNT	= 3;

	[SerializeField]
	private GameObject[] m_Effects = new GameObject[EFFECT_COUNT];

	///--------------------------------
	// エフェクト再生
	///--------------------------------
	public void PlayEffect( int idx, Vector3 position )
	{
		if( idx < EFFECT_GOOD || idx >= EFFECT_COUNT )
		{
			return;
		}

		var effect = Instantiate( m_Effects[idx], position, m_Effects[idx].transform.rotation ) as GameObject;
		StartCoroutine( "DeleteEffect", effect );
	}

	///--------------------------------
	// 消去
	///--------------------------------
	IEnumerator DeleteEffect( GameObject effect )
	{
		yield return new WaitForSeconds( 1.0f );
		Destroy( effect );
	}
}
