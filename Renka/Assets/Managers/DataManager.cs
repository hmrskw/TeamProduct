using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get { return instance; }
    }

    public class Mastering
    {

        //誰を攻略中か
        // 0. 辰巳
        // 1. 酉助
        // 2. 卯太郎
        // 3. 一午
        public int masteringCharacterID;

        //好感度
        public int likeabillity;

        //何話まで読み終わっているか
        public int finishedReadStoryID;

        //そのキャラクターのストーリーが何話まであるか
        public int masteringCharacterLastStoryID;

        public Mastering() {
            masteringCharacterLastStoryID = 0;
            likeabillity = 0;
            finishedReadStoryID = 0;
            masteringCharacterID = -1;
        }
    }

    public Mastering masteringData;

    //今何話を読んでいるか
    public int nowReadStoryID;

    //今何行目まで読んだか
    public int endLine;

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
        Init();
        LoadMasteringData();
    }

    public void Init()
    {
        masteringData = new Mastering();
        nowReadStoryID = -1;
        endLine = 0;
        isChoiceText = false;
    }

    public void LoadMasteringData()
    {
        if (SaveData.ContainsKey("masteringData"))
        {
            masteringData = SaveData.GetClass<Mastering>("masteringData", new Mastering());
            nowReadStoryID = masteringData.finishedReadStoryID;
        }
        else
        {
            Debug.Log("セーブデータが見つかりませんでした。");
        }
    }

    public void SaveMasteringData()
    {
        masteringData.finishedReadStoryID = nowReadStoryID;
        SaveData.SetClass<DataManager.Mastering>("masteringData", DataManager.Instance.masteringData);
        SaveData.Save();
    }

    public void ResetMasteringData()
    {
        if (SaveData.ContainsKey("masteringData"))
        {
            SaveData.Clear();
            Debug.Log("セーブデータを削除しました");
        }
        else
        {
            Debug.Log("セーブデータが見つかりませんでした。");
        }
    }
    public bool isEndStory()
    {
        return masteringData.masteringCharacterLastStoryID <= nowReadStoryID;
    }
}
