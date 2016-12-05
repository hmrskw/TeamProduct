using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChapterNode : MonoBehaviour
{

	[SerializeField, Tooltip("(仮)章データ")]
	ChapterData chapterData;

	[SerializeField, Tooltip("自身のトランスフォーム")]
	RectTransform rectTrans;

	[SerializeField, Tooltip("自身のレイアウト属性")]
	LayoutElement layout;

	[SerializeField, Tooltip("章の名前を入れる")]
	Text chapterName;

	[SerializeField, Tooltip("Episodeのプレハブ")]
	GameObject episodePrefab;

	/// <summary>
	/// 必要数だけ生成して管理する
	/// </summary>
	Episode[] episodes;


	void Start()
	{
		//Setup(chapterData);
	}


	/// <summary>
	/// チャプターデータから
	/// </summary>
	/// <param name="data"></param>
	public void Setup(ChapterData data)
	{
		//var size = data.episodes.Length;
		//var name(Cha) = data.name;
		//var name(Epi) = data.episodes[0].name;
		Debug.Log(" Data Name :" + data.name);
		//Debug.Log(" data  name :" + data.episodes[0].name);

		chapterName.text = data.name;

		//話の生成
		episodes = new Episode[data.episodes.Length];
		Debug.Log(" epsodes create[]");
		for (var i = 0; i < data.episodes.Length; ++i)
		{
			Debug.Log("Create Epsode " + i);
			var obj = (GameObject)Instantiate(episodePrefab, transform);

			//参照の中心となるスクリプトを取得
			var script = obj.GetComponent<Episode>();

			//話の名前をデータから入力
			//script.SetName(data.episodes[i].name);
			script.Setup(data.episodes[i]);

			//話の座標を設定
			script.transform.localScale = Vector3.one;
			//script.transform.localPosition = Vector3.zero;
			
			//script.RectTrans.anchoredPosition = new Vector2(0f, -150f) + Vector2.down * 100f * i;
			script.RectTrans.anchoredPosition = new Vector2(0f, -150f) + Vector2.down * 120f * i;

			//Vector3.down * i * 100f;

			episodes[i] = script;
		}

		var size = data.episodes.Length;

		//話の数でサイズが変わる (章の幅)+(話の幅)*(話数の半分)
		//rectTrans.offsetMax = new Vector2( 800f, 100f + (size/2+ size%2)* 200f);
		//layout.minHeight = 100f + (size / 2 + size % 2) * 200f;
		//layout.minHeight = 100f + size * 100f;
		layout.minHeight = 140f + size * 120f;

	}



}
