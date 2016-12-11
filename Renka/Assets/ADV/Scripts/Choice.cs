using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Choice : MonoBehaviour
{

    //テキストオブジェクト参照
    [SerializeField]
    public GameObject TextObj;

    //[SerializeField]
    public Button button;

    //テキスト上書きするための参照
    public Text text;

    public int point{ set; get; } 

    //ボタンを離したかどうかの変数
    public bool isReleased { get; set; }

    /// <summary>
    /// 初期化処理
    /// Startメソッドが呼ばれないので
    /// </summary>
    public void Setup()
    {
        isReleased = false;
        button = GetComponent<Button>();
        text = TextObj.GetComponent<Text>();
    }

    public void AddPoint()
    {
        DataManager.Instance.masteringData.likeabillity += point;
        isReleased = true;
    }

    /// <summary>
    /// 次の選択肢用にリセット
    /// </summary>
    public void Reset()
    {
        isReleased = false;
    }

    public void SetCanPush(bool canPush)
    {
        button.interactable = canPush;
    }

    /// <summary>
    /// 離す、ボタンを離した時呼ばれるる
    /// </summary>
    public void Release()
    {
        isReleased = true;
    }
}
