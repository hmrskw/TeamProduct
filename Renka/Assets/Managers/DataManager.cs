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

        //そのキャラクターのストーリーが何章まであるか
        public int masteringCharacterLastChapterID;

        public MasteringData() {
            masteringCharacterLastStoryID = 0;
            masteringCharacterLastChapterID = 0;
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

    /// <summary>
    /// ADVに使うデータ
    /// </summary>
    //今何章を読んでいるか
    public int nowReadChapterID;

    //今何話を読んでいるか
    public int nowReadStoryID;
    
    //回想用:選択されたキャラクター
    public int nowReadCharcterID;

    //今何行目まで読んだか
    public int endLine;

    //今読んでいるのは選択肢後の分岐かどうか
    public bool isChoiceText;

    /// <summary>
    /// この値より大きくなると恋情ルートに行ける
    /// </summary>
    public int baseline = 5;
    /// <summary>
    /// ミニゲームに使うデータ
    /// </summary>
    //ミニゲーム終了時の残り体力
    public int minigameHp;

    public bool[] item = new bool[3];

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
            Debug.Log(SceneChanger.GetBeforeSceneName());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(
                "bgm" + configData.bgm +
                "\nse" + configData.se +
                "\nvoice" + configData.voice +
                "\ntextBox" + configData.textBox +
                "\ntextSpd" + configData.textSpd +
                "\nenableSkip" + configData.isSkip
            );

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Chapter" + finishedStoryData[0].finishedReadChapterID + "\nStory" + finishedStoryData[0].finishedReadStoryID);
            Debug.Log("Chapter" + finishedStoryData[1].finishedReadChapterID + "\nStory" + finishedStoryData[1].finishedReadStoryID);
            Debug.Log("Chapter" + finishedStoryData[2].finishedReadChapterID + "\nStory" + finishedStoryData[2].finishedReadStoryID);
            Debug.Log("Chapter" + finishedStoryData[3].finishedReadChapterID + "\nStory" + finishedStoryData[3].finishedReadStoryID);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Likeabillity" + masteringData.likeabillity + "\nMasteringCharacterID" + masteringData.masteringCharacterID);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("NowReadStory" + nowReadStoryID + "NowReadChapter" + nowReadChapterID);
        }

        //Stop
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.Space))
                SoundManager.Instance.StopSE(true);
            else
                SoundManager.Instance.StopSE();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.Space))
                SoundManager.Instance.StopBGM(true);
            else
                SoundManager.Instance.StopBGM();
        }

        //Volume
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.Instance.ChangeSEVolume(SoundManager.Instance.GetSEVolume() + 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.Instance.ChangeSEVolume(SoundManager.Instance.GetSEVolume() - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.GetBGMVolume() + 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.GetBGMVolume() - 0.1f);
        }

        //SE
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SoundManager.Instance.PlaySE("fire");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SoundManager.Instance.PlaySE("hit");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SoundManager.Instance.PlaySE("taiko");
        }

        //BGM
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoundManager.Instance.PlayBGM("Kasinomai");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SoundManager.Instance.PlayBGM("Sakuya3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SoundManager.Instance.PlayBGM("Tukiyatyou");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SoundManager.Instance.PlayBGM("DearChildhoodFriend");
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
        nowReadCharcterID = -1;
        nowReadStoryID = -1;
        nowReadChapterID = -1;
        endLine = 0;
        isChoiceText = false;

        minigameHp = 3;
    }

    public bool isEndStory()
    {
        return masteringData.masteringCharacterLastStoryID <= nowReadStoryID;
    }

    public bool isEndChapter()
    {
        return masteringData.masteringCharacterLastChapterID <= nowReadChapterID + 1;
    }
}
