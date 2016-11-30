using UnityEngine;
using System.Collections;

public class Record : MonoBehaviour
{

	[SerializeField, Tooltip("章ノードを生成する場所")]
	GameObject contents;

	[SerializeField, Tooltip("(仮)記録帖一ページ分のデータ")]
	RecordData recordData;

	[SerializeField]
	GameObject chapterNodePrefab;

	ChapterNode[] chapterNodes;

	void Start()
	{
		//SetupRecord(recordData);
	}

	/// <summary>
	/// ChapterNodeを生成してContentsにぶっこむ
	/// </summary>
	/// <param name="data"></param>
	public void SetupRecord( RecordData data )
	{
		Debug.Log("SetupRecord : " + data.name);
		//var chapSize = recordData.chapters.Length;
		//var chapName = recordData.chapters[0].name;
		//var epiSize = recordData.chapters[0].episodes.Length;
		//var epiName = recordData.chapters[0].episodes[0].name;

		//データのサイズ分だけ章を生成
		var size = data.chapters.Length;
		chapterNodes = new ChapterNode[size];
		for (var i = 0; i < size; ++i)
		{
			var obj = Instantiate<GameObject>(chapterNodePrefab);

			//スクリプトの取得
			var script = obj.GetComponent<ChapterNode>();
			chapterNodes[i] = script;

			//セットアップ
			script.Setup(data.chapters[i]);

			//子にする
			script.transform.parent = contents.transform;

			//大きさの初期化
			script.transform.localScale = Vector3.one;
		}

	}

	//transform.parent

}
