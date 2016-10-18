using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Choice : MonoBehaviour
{

    //テキストオブジェクト参照
    [SerializeField]
    public GameObject TextObj;

    //テキスト上書きするための参照
    public Text text;

    //ボタンUIから
    //public Button button;

    public int point{
        set;get;
    } 

    //ボタンを離したかどうかの変数
    public bool isReleased { get; set; }
    //int pushCount;

    /// <summary>
    /// 初期化処理
    /// Startメソッドが呼ばれないので
    /// </summary>
    public void Setup()
    {
        isReleased = false;
        text = TextObj.GetComponent<Text>();
    }

    /// <summary>
    /// 次の選択肢用にリセット
    /// </summary>
    public void Reset()
    {
        isReleased = false;
        Debug.Log("ResetChoice");
    }

    /// <summary>
    /// 離す、ボタンを離した時呼ばれるる
    /// </summary>
    public void Release()
    {
        isReleased = true;
        Debug.Log("ReleaseChoice");
    }

}
