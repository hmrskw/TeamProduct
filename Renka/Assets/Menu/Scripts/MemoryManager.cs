using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//未使用
[System.Serializable]
public class StillData
{
	[SerializeField,Tooltip("名前")]
	string name;

	[SerializeField]
	string fileName;
}

//未使用
[System.Serializable]
public class StillPage
{
	[SerializeField, Tooltip("名前")]
	public string name;
	public StillData[] stills;	
}

/// <summary>
/// 各キャラクターページごとにまとめたクラス
/// </summary>
[System.Serializable]
public class CharaPage
{
	[SerializeField, Tooltip("名前")]
	string name;

	[SerializeField, Tooltip("しおりのボタンの参照")]
	Button bookmarkButton;
	public Button BookmarkButton
	{
		get { return bookmarkButton; }
	}

	[SerializeField, Tooltip("一ページ分をまとめたスクリプト")]
	Memory memoryScript;
	public Memory MemoryScript
	{
		get { return memoryScript; }
	}

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="page">一ページ分のデータ</param>
	/// <param name="disCol">獲得してないスチルのカラー</param>
	public void Setup(Page page, Color disCol )
	{
		//Debug.Log("Setup " + name);
		//Debug.Log("NumImages " + memoryScript.images.Length);
		//Debug.Log("NumStills " + page.Stills.Length);

		//スチルを画像オブジェに割り当てる前に、全ての画像オブジェを非表示に
		foreach (var item in memoryScript.images)
		{
			item.enabled = false;
		}

		//表示に必要な画像だけ割り当て、表示状態にする
		for(var i = 0; i < page.Stills.Length; i++)
		{
			//4枚以上は今のとこなし
			if (i >= memoryScript.images.Length) break;

			//スチルを割り当てる
			memoryScript.images[i].texture = page.Stills[i].Tex;

			//Debug.Log("TexName : " + memoryScript.images[i].texture.name);


			//獲得してなければ専用の色を暗く
			if (page.Stills[i].IsHave == false)
			{
				//var mat = new Material();
				//memoryScript.images[i].material.color = disCol;
				//memoryScript.images[i].material = disMat;
				//memoryScript.images[i].material.SetColor("_Color", new Color(0.5f,0.5f,0.5f,1));
				memoryScript.images[i].color = disCol;
			}
			else
			{
				memoryScript.images[i].material.color = Color.white;
			}

			//表示する
			memoryScript.images[i].enabled = true;
		}

		
	}
}

/// <summary>
/// 一人分のスチルデータ
/// </summary>
[System.Serializable]
public class Page
{
	[SerializeField, Tooltip("名前")]
	string name;

	[SerializeField]
	Still[] stills;
	public Still[] Stills
	{
		get { return stills; }
		set { stills = value; }
	}

	//[SerializeField]
	//int enableNumStills;
	//public int EnableNumStills
	//{
	//	get { return enableNumStills; }
	//	set { enableNumStills = value; }
	//}

}

/// <summary>
/// スチルごとのデータ
/// </summary>
[System.Serializable]
public class Still
{ 

	/// <summary>
	///スチルの名前
	/// </summary>
	[SerializeField, Tooltip("スチルの名前")]
	string name;

	//グラフィック
	/// <summary>
	/// テキストなしスチル
	/// </summary>
	[SerializeField]
	Texture tex;
	public Texture Tex
	{
		get {return tex; }
		private set { tex = value; }
	}

	/// <summary>
	/// テキストありスチル
	/// </summary>
	[SerializeField]
	Texture tex2;
	public Texture Tex2
	{
		get { return tex2; }
		private set { tex2 = value; }
	}

	//入手できる話(キャラ-章-話)
	public int GetCharID;
	public int GetChapID;
	public int GetEpiID;

	//攻略データから
	public void SetIsHaveFromCaptureData(DataManager.FinishedStoryData[] data)
	{
		if (GetChapID > data[GetCharID].finishedReadChapterID)
		{//この章まで攻略されてない
			isHave = false;
		}
		else if (GetChapID < data[GetCharID].finishedReadChapterID)
		{//この章は攻略し終えた
			isHave = true;
		}
		//☆セーブの仕様がわからんからスチル獲得が不明
		else if (GetEpiID >= data[GetCharID].finishedReadStoryID)
		{//この章のこの話まで攻略されてない
			isHave = false;
		}
		else
		{
			isHave = true;
		}
	}

	/// <summary>
	///獲得して見れるかどうか
	/// </summary>
	[SerializeField]
	bool isHave;
	public bool IsHave
	{
		get {return isHave; }
		private set { isHave = value; }
	}
}

public class MemoryManager : MonoBehaviour
{

	/// <summary>
	/// キャラクター種類
	/// </summary>
	public enum MemoryName
	{
		TATSUMI = 0,
		YUUSUKE,
		OTHER,
		NUM,
	}


	[SerializeField, Tooltip("参照用")]
	CharaPage[] charaPages;

	[SerializeField, Tooltip("ページごとのデータ")]
	Page[] pages;

	[SerializeField, Tooltip("獲得してないスチルの色")]
	Color disableStillColor;

	[SerializeField, Tooltip("獲得してないスチルの色")]
	Material disableStillMaterial;

	[SerializeField, Tooltip("そのページが表示される")]
	int select;

	[SerializeField, Tooltip("栞をどのくらい下げて隠すか")]
	float bookmarkDown;

	[SerializeField, Tooltip("スチル表示ようオブジェクト")]
	StillManager stillScript;

	void Awake()
	{
		select = 0;
	}

	void Start()
	{

		//スチルの獲得情報を攻略データから入力
		//DataManager.Instance.finishedStoryData[0]
		//var a = DataManager.Instance.finishedStoryData;
		//pages
		for(var i = 0; i < pages.Length; i++)
		{
			for(var j = 0; j < pages[i].Stills.Length; j++)
			{
				pages[i].Stills[j].SetIsHaveFromCaptureData( DataManager.Instance.finishedStoryData );
			}

		}



		//キャラページごとに初期化
		for (var i = 0; i < charaPages.Length; ++i)
		{
			//ページのデータ数が足りない場合
			if (pages.Length <= i) break;

			//初期化
			charaPages[i].Setup(pages[i], disableStillColor);

		}

		SetupMemory();

	}

	void Update()
	{
		//charaPages[0].MemoryScript.ButtonClick();
	}

	/// <summary>
	/// 思い出表示の初期化
	/// </summary>
	void SetupMemory()
	{
		//select = (int)MemoryName.TATSUMI;

		//しおりページをすべてオフにする
		foreach (var item in charaPages )
		{
			item.MemoryScript.gameObject.SetActive(false);
			//item.BookmarkButton.gameObject.SetActive(false);
			var t = item.BookmarkButton.gameObject.GetComponent<RectTransform>();
			var pos = t.anchoredPosition;
			t.anchoredPosition = new Vector2(pos.x, -bookmarkDown);
		}

		//選択されたしおりのページのみ表示
		charaPages[select].MemoryScript.gameObject.SetActive(true);
		//charaPages[select].BookmarkButton.transform.Translate(0, 150,0);
		var rt_ = charaPages[select].BookmarkButton.gameObject.GetComponent<RectTransform>();
		var pos_ = rt_.anchoredPosition;
		rt_.anchoredPosition = new Vector2(pos_.x, 0f);

	}

	/// <summary>
	///	しおりIDに対応したページを表示する、それ以外のページを非表示にする
	///	おもにしおりをクリックしたときに呼ばれる
	/// </summary>
	/// <param name="id">ID</param>
	public void ChangeMemory( int id )
	{
		select = id;
		//Debug.Log("ChangeMemory : " + id);
		for(var i = 0; i < charaPages.Length; ++i)
		{

			if (id == i)
			{
				charaPages[i].MemoryScript.gameObject.SetActive(true);
				var t = charaPages[i].BookmarkButton.gameObject.GetComponent<RectTransform>();
				var pos = t.anchoredPosition;
				t.anchoredPosition = new Vector2(pos.x, 0f);
			}
			else
			{
				charaPages[i].MemoryScript.gameObject.SetActive(false);
				var t = charaPages[i].BookmarkButton.gameObject.GetComponent<RectTransform>();
				var pos = t.anchoredPosition;
				t.anchoredPosition = new Vector2(pos.x, -bookmarkDown);
			}

		}
        SoundManager.Instance.PlaySE("kirokuchou");
    }


	/// <summary>
	/// IDにあうスチルの表示
	/// 各ページ0~4
	/// </summary>
	/// <param name="name_"></param>
	/// <param name="id_"></param>
	public void OnClickTatsumiStill(int id_)
	{
		Debug.Log("OnClickTatsumiStill :" + id_);

		//獲得してない画像なら表示しない
		if (pages[(int)MemoryName.TATSUMI].Stills[id_].IsHave == false) return;
  
		//スチルをセットする
		stillScript.tex1 = pages[(int)MemoryName.TATSUMI].Stills[id_].Tex;
		stillScript.tex2 = pages[(int)MemoryName.TATSUMI].Stills[id_].Tex2;

		stillScript.stillImage1.texture = stillScript.tex1;
		stillScript.stillImage2.texture = stillScript.tex2;

		//スチルキャンバスをアクティブ化
		stillScript.gameObject.SetActive(true);

	}

	public void OnClickYuusukeStill(int id_)
	{
		Debug.Log("OnClickYuusukeStill :" + id_);

		if (pages[(int)MemoryName.YUUSUKE].Stills[id_].IsHave == false) return;

		//スチルをセットする
		stillScript.tex1 = pages[(int)MemoryName.YUUSUKE].Stills[id_].Tex;
		stillScript.tex2 = pages[(int)MemoryName.YUUSUKE].Stills[id_].Tex2;

		stillScript.stillImage1.texture = stillScript.tex1;
		stillScript.stillImage2.texture = stillScript.tex2;

		//スチルキャンバスをアクティブ化
		stillScript.gameObject.SetActive(true);

	}

	public void OnClickOtherStill(int id_)
	{
		Debug.Log("OnClickOtherStill :" + id_);

		if (pages[(int)MemoryName.OTHER].Stills[id_].IsHave == false) return;

		//スチルをセットする
		stillScript.tex1 = pages[(int)MemoryName.OTHER].Stills[id_].Tex;
		stillScript.tex2 = pages[(int)MemoryName.OTHER].Stills[id_].Tex2;

		stillScript.stillImage1.texture = stillScript.tex1;
		stillScript.stillImage2.texture = stillScript.tex2;

		//スチルキャンバスをアクティブ化
		stillScript.gameObject.SetActive(true);

	}
}
