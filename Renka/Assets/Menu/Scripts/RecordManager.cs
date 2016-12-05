using UnityEngine;
using System.Collections;

/// <summary>
/// キャラごとの記録ページ
/// </summary>
[System.Serializable]
public class RecordData
{
	public string name;

	public ChapterData[] chapters;
}

/// <summary>
/// 章のデータ
/// </summary>
[System.Serializable]
public class ChapterData
{
	//章の名前
	public string name;

	public EpisodeData[] episodes;

}

/// <summary>
/// 話(わ)のデータ
/// </summary>
[System.Serializable]
public class EpisodeData
{
	//話の名前
	public string name;

	//リンク先
	public string path;

    //キャラクター番号
    public int charID;

    //章の番号
    public int chapID;

    //話の番号
    public int epiID;

	//獲得してるかどうか
	public bool isHave;

}

/// <summary>
/// 記録帖のキャラごとの参照をまとめたクラス
/// </summary>
[System.Serializable]
public class RecordObject
{
	[SerializeField]
	string name;

	public Bookmark bookmark;

	public GameObject record;

	public Record recordScript;

}

/// <summary>
/// 記録帖を管理する
/// </summary>
public class RecordManager : MonoBehaviour
{

	[SerializeField, Tooltip("記録帖を生成する際の全てのデータ")]
	RecordData[] records;

	[SerializeField, Tooltip("記録帖の挙動を管理するための参照")]
	RecordObject[] recordObjects;

	[SerializeField, Tooltip("しおりを下げる距離")]
	float bookmarkDiff;

	//現在選択されてるしおりのID
	//辰己:0, 酉助:1, その他:2
	int bookmarkID;

	//シーン生成時に決まる最初のID
	[SerializeField]
	int initID;
	public int InitID
	{
		get { return initID; }
		set { initID = value; }
	}

	void Start()
	{
		SetupBookmarks();
		SetupRecordContents();
		ChangePage(initID);
	}

	/// <summary>
	/// しおりの初期化
	/// </summary>
	void SetupBookmarks()
	{
		//Debug.Log("SetupBookmarks");

		//全てのしおりを非選択状態にする
		for (var i = 0; i < recordObjects.Length; ++i)
		{
			//名前が書いてある画像のオフ
			recordObjects[i].bookmark.imageName.enabled = false;
			//しおりの座標を下げる
			var pos = recordObjects[i].bookmark.image.rectTransform.anchoredPosition;
			pos.y += -bookmarkDiff;
			recordObjects[i].bookmark.image.rectTransform.anchoredPosition = pos;
		}

		//id:0のしおりを選択上にする
		recordObjects[0].bookmark.imageName.enabled = true;
		var pos_ = recordObjects[0].bookmark.image.rectTransform.anchoredPosition;
		pos_.y += bookmarkDiff;
		recordObjects[0].bookmark.image.rectTransform.anchoredPosition = pos_;

		bookmarkID = 0;
	}

	/// <summary>
	/// しおりページを変えるときの処理
	/// </summary>
	/// <param name="id">表示するページのID</param>
	void ChangePage( int id )
	{
		//Debug.Log("ChageBookmark id :" + id );

		if (id == bookmarkID) return;

		///////////////////////////////////////////////////////////////////////////////////////////
		//しおりの処理
		//現在のしおりを非選択状態にする
		recordObjects[bookmarkID].bookmark.imageName.enabled = false;
		var pos1 = recordObjects[bookmarkID].bookmark.image.rectTransform.anchoredPosition;
		pos1.y += -bookmarkDiff;
		recordObjects[bookmarkID].bookmark.image.rectTransform.anchoredPosition = pos1;

		//idのしおりを選択状態にする
		recordObjects[id].bookmark.imageName.enabled = true;
		var pos2 = recordObjects[id].bookmark.image.rectTransform.anchoredPosition;
		pos2.y += bookmarkDiff;
		recordObjects[id].bookmark.image.rectTransform.anchoredPosition = pos2;

		///////////////////////////////////////////////////////////////////////////////////////////
		//ゲームオブジェクトの処理

		recordObjects[ bookmarkID ].record.SetActive(false);

		recordObjects[ id ].record.SetActive(true);

		///////////////////////////////////////////////////////////////////////////////////////////

		//bookmarkIDの更新
		bookmarkID = id;

	}

	/// <summary>
	/// 記録帖ページを全て作成する
	/// </summary>
	void SetupRecordContents()
	{
        //記録帖を生成する前にデータに
        //キャラ、章、話のIDを振り分ける
        for (var i = 0; i < records.Length; i++)
        {
            for (var j = 0; j < records[i].chapters.Length; j++)
            {
                for (var k = 0; k < records[i].chapters[j].episodes.Length; k++)
                {
					Debug.Log("ijk"+ i+ " : " + j + " : " + k);
                    records[i].chapters[j].episodes[k].charID = i;
                    records[i].chapters[j].episodes[k].chapID = j;
                    records[i].chapters[j].episodes[k].epiID  = k;
                }
            }
        }

		//Debug.Log("SetupRecordContents");
		Debug.Log("length : " + recordObjects.Length);

		//記録帖の動的オブジェクトを生成
		for (var i = 0; i < recordObjects.Length; ++i)
		{
			if (recordObjects[i].recordScript.enabled)
			{
				recordObjects[i].recordScript.SetupRecord(records[i]);
			}
		}
	}

	/// <summary>
	/// しおりがクリックされたときの処理
	/// </summary>
	/// <param name="id"></param>
	public void OnClickBookmark(int id)
	{
		Debug.Log("しおり id : " + id);
		ChangePage(id);
	}

	public void OnClickEpsode( int charaID, int chatID, int epiID)
	{

	}
}
