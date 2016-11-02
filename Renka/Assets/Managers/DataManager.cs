using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get { return instance; }
    }

    //誰を攻略中か
    // 0. 辰巳
    // 1. 酉助
    // 2. 卯太郎
    // 3. 一午
    public int masteringCharacterID;

    //好感度
    public int likeabillity { set; get; }

    //今何話を読んでいるか
    public int nowReadStoryID;

    //今何行目まで読んだか
    public int endLine { set; get; }

    //今読んでいるのは選択肢後の分岐かどうか
    public bool isChoiceText;

    //シーンまたいでもオブジェクトが破棄されなくする
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        init();
    }

    public void init()
    {
        masteringCharacterID = -1;
        likeabillity = 0;
        endLine = 0;
        isChoiceText = false;
    }
}
