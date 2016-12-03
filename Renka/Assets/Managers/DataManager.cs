using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get { return instance; }
    }

    public class MasteringData
    {
        //誰を攻略中か
        // 0. 辰巳
        // 1. 酉助
        // 2. 卯太郎
        // 3. 一午
        public int masteringCharacterID;

        //好感度
        public int likeabillity;

        //そのキャラクターのストーリーが何話まであるか
        public int masteringCharacterLastStoryID;
        
        public MasteringData() {
            masteringCharacterLastStoryID = 0;
            likeabillity = 0;
            masteringCharacterID = -1;
        }
    }

    public class FinishedStoryData
    {
        //何章まで読み終わったか
        public int finishedReadChapterID;

        //何話まで読み終わっているか
        public int finishedReadStoryID;

        public FinishedStoryData()
        {
            finishedReadStoryID = 0;
            finishedReadChapterID = 0;
        }
    }

    public class ConfigData
    {
        //[SerializeField, Range(0f, 1f), Tooltip("BGMの大きさ")]
        public float bgm;
        //public float BGM { get { return bgm; } set { bgm = value; } }

        //[SerializeField, Range(0f, 1f), Tooltip("SEの大きさ")]
        public float se;
        //public float SE { get { return se; } set { se = value; } }

        //[SerializeField, Range(0f, 1f), Tooltip("VOICEの大きさ")]
        public float voice;
        //public float VOICE { get { return voice; } set { voice = value; } }

        //[SerializeField, Range(0f, 1f), Tooltip("テキストボックスの透明度")]
        public float textBox;
        //public float TextBox { get { return textBox; } set { textBox = value; } }

        //[SerializeField, Range(0f, 1f), Tooltip("テキストスピード")]
        public float textSpd;
        //public float TextSpd { get { return textSpd; } set { textSpd = value; } }

        //[SerializeField, Tooltip("既読スキップの有無")]
        public bool isSkip;
        //public bool IsSkip { get { return isSkip; } set { isSkip = value; } }
    }

    public MasteringData masteringData;
    public FinishedStoryData[] finishedStoryData;// = new FinishedStoryData[4];
    public ConfigData configData;

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

        SaveData.LoadMasteringData();
        //Debug.Log("LoadMastering" +     DataManager.Instance.masteringData.masteringCharacterID);
        SaveData.LoadFinishedStoryData();
        //Debug.Log("LoadFinishedStory" + DataManager.Instance.masteringData.masteringCharacterID);
        SaveData.LoadConfigData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(
                "bgm" + DataManager.Instance.configData.bgm +
                "\nse" + DataManager.Instance.configData.se +
                "\nvoice" + DataManager.Instance.configData.voice +
                "\ntextBox" + DataManager.Instance.configData.textBox +
                "\ntextSpd" + DataManager.Instance.configData.textSpd +
                "\nenableSkip" + DataManager.Instance.configData.isSkip
            );

        }
    }
    public void Init()
    {
        masteringData = new MasteringData();
        finishedStoryData = new FinishedStoryData[4];
        configData = new ConfigData();
        for (int i = 0;i<4; i++)
        {
            finishedStoryData[i] = new FinishedStoryData();
        }
        nowReadStoryID = -1;
        endLine = 0;
        isChoiceText = false;
    }

    public bool isEndStory()
    {
        return masteringData.masteringCharacterLastStoryID <= nowReadStoryID;
    }
}
