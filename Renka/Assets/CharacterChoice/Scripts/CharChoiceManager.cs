using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharChoiceManager : MonoBehaviour
{

    [SerializeField, Tooltip("辰巳を選択できるか")]
    bool canSelectsTatsumi;

    [SerializeField, Tooltip("酉助を選択できるか")]
    bool canSelectsYuusuke;

    [SerializeField, Tooltip("卯太郎を選択できるか")]
    bool canSelectsUtarou;

    [SerializeField, Tooltip("一午を選択できるか")]
    bool canSelectsKazuma;

    [SerializeField, Tooltip("辰巳のボタンスクリプト")]
    Button tatsumi;

    [SerializeField, Tooltip("酉助のボタンスクリプト")]
    Button yuusuke;

    [SerializeField, Tooltip("卯太郎のボタンスクリプト")]
    Button utarou;

    [SerializeField, Tooltip("一午のボタンスクリプト")]
    Button kazuma;

    void Start()
    {
        //選べないキャラを選択できなくする
        if (canSelectsTatsumi == false)
        {
            //tatsumi.enabled = false;
            //tatsumi.gameObject.SetActive(false);
            tatsumi.interactable = false;
        }

        if (canSelectsYuusuke == false)
        {
            yuusuke.interactable = false;
        }

        if (canSelectsUtarou == false)
        {
            utarou.interactable = false;
        }

        if (canSelectsKazuma == false)
        {
            kazuma.interactable = false;
        }

    }

    /// <summary>
    /// ボタンを押したときの処理
    /// 攻略キャラクターの設定とシーンの切り替え
    /// </summary>
    public void OnClickTatsumi()
    {
        //Debug.Log("Click Tatsumi");
        DataManager.Instance.masteringCharacterID = 0;
        //Debug.Log("攻略キャラID : " + DataManager.Instance.masteringCharacterID);
        ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
        SceneChanger.LoadScene("MyPage",false);
    }

    public void OnClickYuusuke()
    {
        //Debug.Log("Click Yuusuke");
        DataManager.Instance.masteringCharacterID = 1;
        //Debug.Log("攻略キャラID : " + DataManager.Instance.masteringCharacterID);
        ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
        SceneChanger.LoadScene("MyPage",false);
    }

    public void OnClickUtarou()
    {
        //Debug.Log("Click Utarou");
        DataManager.Instance.masteringCharacterID = 2;
        //Debug.Log("攻略キャラID : " + DataManager.Instance.masteringCharacterID);
        ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
        SceneChanger.LoadScene("MyPage",false);
    }

    public void OnClickKazuma()
    {
        //Debug.Log("Click Kazuma");
        DataManager.Instance.masteringCharacterID = 3;
        //Debug.Log("攻略キャラID : " + DataManager.Instance.masteringCharacterID);
        ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
        SceneChanger.LoadScene("MyPage",false);
    }
}