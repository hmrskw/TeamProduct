using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Episode : MonoBehaviour
{

	[SerializeField, Tooltip("子のテキストを設定")]
	Text text;

	[SerializeField, Tooltip("自身のトランスフォーム")]
	RectTransform rectTrans;
	public RectTransform RectTrans { get { return rectTrans; } }

	[SerializeField, Tooltip("自身のボタン")]
	Button button;

    //この話のキャラのID
    public int CharID { get; private set; }

    //この話の章のID
    public int ChapID { get; private set; }

    //この話のID
    public int EpiID { get; private set; }

    bool isHave;
	public bool IsHave
	{
		get { return isHave; }
		set { isHave = value; }
	}

	void Start()
	{

        //Button button = this.GetComponent<Button>();
        //button.onClick.AddListener(() => { Debug.Log("Clicked." + text.text); });
        button.onClick.AddListener(() => { Debug.Log("OnClick " + "キャラ " + CharID + ", " + ChapID + "章, " + EpiID + "話"); });
        //string str = "キャラ " + CharID +", " + ChapID + "章, "+ EpiID + "話";


        //var num = text.text.Length;
        //※仕様変更
        //文字列が長い場合　ボタンを大きくする
        //if(num > 3)
        //{
        //	var min = RectTrans.offsetMin;
        //	var max = RectTrans.offsetMax;
        //	//RectTrans.offsetMax =  new Vector2( num * 50f + 50f  ,1f);
        //	RectTrans.offsetMin = new Vector2(-(num * 50f + 50f)/2, min.y);
        //	RectTrans.offsetMax = new Vector2((num * 50f + 50f)/2,  max.y);
        //}

        //ボタンを大きめに作る
        var min = RectTrans.offsetMin;
		var max = RectTrans.offsetMax;
		var num = 8f;
		RectTrans.offsetMin = new Vector2(-(50f)*num / (float)2, min.y);
		RectTrans.offsetMax = new Vector2( (50f)*num / (float)2, max.y);


	}

	public void Setup(string name = "N")
	{
		//Button button = this.GetComponent<Button>();
		button.onClick.AddListener(() => { Debug.Log("Click : " + name + " : " + text.text  ); });
	}

	public void SetName( string name)
	{
		text.text = name;
	}

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="data">初期化を行うためのデータ</param>
	public void Setup( EpisodeData data )
	{

        //どの話がわかるデータ類
        CharID = data.charID;
        ChapID = data.chapID;
        EpiID = data.epiID;

        //データマネージャーの攻略情報から
        //この話を取得しているかを判別する
        var finData = DataManager.Instance.finishedStoryData[CharID];
        if (ChapID > finData.finishedReadChapterID)
        {
            isHave = false;
        }
        else if (EpiID >= finData.finishedReadStoryID)
        {
            isHave = false;
        }
        else
        {
            isHave = true;
        }

        //isHave = data.isHave;

        if (isHave)
		{
			text.text = data.name;
		}
		else
		{
			text.text = "？？？";
			button.interactable = false;
		}
	}


}
