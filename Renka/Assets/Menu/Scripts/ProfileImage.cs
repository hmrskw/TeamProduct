using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfileImage : MonoBehaviour
{

    public int Index { get; private set; }

    [SerializeField]
    float limit;

	//[SerializeField]
	//RectTransform rectTrans;
    public RectTransform RectTrans  { get; private set; }

    //ボタン
    void Awake()
    {
        RectTrans = GetComponent<RectTransform>();
    }

    /// <summary>
    /// このオブジェクトがクリックされたとき呼ばれる
    /// </summary>
    public void OnClick()
    {
        Debug.Log("クリックプロフィール");
    }

}
