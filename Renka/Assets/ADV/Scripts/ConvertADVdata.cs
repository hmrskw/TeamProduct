using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System; 

public class ConvertADVdata : MonoBehaviour {
    private static ConvertADVdata instance;

    public static ConvertADVdata Instance
    {
        get { return instance; }
    }
    ReadCSV readCsv;

    public class ADVData
    {
        public string command;//コマンド
        public string parameter;//コマンドに使うパラメーター

        public int drawCharacterNum;//表示するキャラクターの数
        public int[] drawCharacterID = new int[2];//キャラクターのID
        public int[] expression = new int[2];//表情
        public int[] costume = new int[2];//服装
        public Vector2[] pos = new Vector2[2];//表示位置
        public float[] size = new float[2];//表示サイズ

        public int choiceNum;//表示する選択肢の数
        public string[] choiceText = new string[3];//各選択肢の文章
        public int[] choicePoint = new int[3];//各選択肢のポイント
        public int[] choiceTermsParameter = new int[3];//各選択肢のポイント

        public string sendCharacter;//キャラクターのID
        public string text;//テキスト

        //public int backGroundID;//背景のID
        //public int BGMID;//鳴らすBGMのID

        public ADVData()
        {
            init();

            //this.backGroundID = 0;
            //this.BGMID = 0;
        }

        public void init()
        {
            this.command = "";
            this.parameter = "";

            this.drawCharacterNum = 0;
            this.drawCharacterID[0] = 0;
            this.expression[0] = 0;
            this.costume[0] = 0;
            this.pos[0] = Vector2.zero;
            this.size[0] = 1f;

            this.choiceNum = 0;
            for (int i = 0; i < 3; i++)
            {
                this.choiceText[i] = "";
                this.choicePoint[i] = 0;
                this.choiceTermsParameter[i] = 0;
            }

            this.sendCharacter = "";
            this.text = "";

            //this.backGroundID = 0;
            //this.BGMID = 0;
        }
    }

    enum ElementsName
    {
        COMMANDO,
        /*
        [send]
            キャラクターのテキスト表示
        [draw]
            キャラクターの描画のみ
        [event]
            イベントCGモード
            イベントCGモード中はキャラクターを描画しない
        [eventEnd]
            イベントCGモードの終了
        [choice] 
            テキストの内容を選択肢にする
            <パラメーター>
                選択肢が選ばれることで入るポイント
        [fade]
            フェードアウト
            <パラメーター>
                フェードアウトにかかる時間   
        [minigame]
            ミニゲームに移動
        */

        PARAMETER,//コマンドで必要なパラメーターの入力欄

        CHARACTER_NAME,//[draw]描画するキャラクターの名前[send]話しているキャラクターの名前

        EXPRESSION,//表情
        COSTUME,//服装
        POSITION_X,//キャラクター描画位置の横
        //POSITION_Y,//キャラクター描画位置の縦
        SIZE,//キャラクター描画サイズ

        TEXT,//表示するテキスト

        //BACK_GROUND,//変更する背景
        //BGM,//鳴らすBGM
        //SE
    }

    [Serializable]
    public struct PositionData
    {
        public float positionX;
        public string positionName;
    }

    [SerializeField]
    PositionData[] positionData;

    [Serializable]
    public struct SizeData
    {
        public float size;
        public string sizeName;
    }

    [SerializeField]
    SizeData[] sizeData;

    [SerializeField]
    string prologueCSV;

    [Serializable]
    public class CSVFiles
    {
        public Chapters[] chapters;

        /*public CSVFiles(string[] storyText)
        {
            StoryText = storyText;
        }*/
    }

    [Serializable]
    public class Chapters
    {
        public string[] StoryText;

        public Chapters(string[] storyText)
        {
            StoryText = storyText;
        }
    }
    //Inspectorに表示される
    [SerializeField]
    private CSVFiles[] csvFile;

    //csvデータの要素数
    const int CSVDATA_ELEMENTS = 8;

    //csvから取り出した情報を入れる配列
    public List<ADVData> advData;

    bool isEventMode = false;

    public List<ADVData> AdvData
    {
        get { return advData; }
    }

    //選択肢での分岐部分のデータ
    public Dictionary<string, List<ADVData>> choiceADVData;

    //描画位置の情報
    Dictionary<string, float> positionDataDictionary = new Dictionary<string, float>();

    //サイズの情報
    Dictionary<string, float> sizeDataDictionary = new Dictionary<string, float>();

    void Awake()
    {
        foreach (PositionData data in positionData) {
            positionDataDictionary.Add(data.positionName,data.positionX);
        }

        foreach (SizeData data in sizeData)
        {
            sizeDataDictionary.Add(data.sizeName, data.size);
        }

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        readCsv = new ReadCSV();

        string[] lines;
        if (DataManager.Instance.masteringData.masteringCharacterID == -1)
        {
            lines = readCsv.ReadFile(prologueCSV + ".csv");
        }
        else if (SceneChanger.GetBeforeSceneName() == "Menu")
        {
            SceneChanger.GetBeforeSceneName(true);
            lines = readCsv.ReadFile(
                csvFile[DataManager.Instance.nowReadCharcterID].
                chapters[DataManager.Instance.nowReadChapterID].
                StoryText[DataManager.Instance.nowReadStoryID] + ".csv");
        }
        else
        {
            Debug.Log(DataManager.Instance.masteringData.masteringCharacterID);
            Debug.Log(DataManager.Instance.nowReadChapterID);
            Debug.Log(DataManager.Instance.nowReadStoryID);
            lines = readCsv.ReadFile(
                csvFile[DataManager.Instance.masteringData.masteringCharacterID].
                chapters[DataManager.Instance.nowReadChapterID].
                StoryText[DataManager.Instance.nowReadStoryID] + ".csv");
        }

        //csvデータの初期化
        advData = new List<ADVData>();

        //選択肢用csvデータの初期化
        choiceADVData = new Dictionary<string, List<ADVData>>();

        //カンマ分けされたデータを仮格納する。その初期化
        string[] didCommaSeparationData = new string[lines.Length];

        //その選択肢が何行目にあるかを保存
        Dictionary<string, int> choiceTiming = new Dictionary<string, int>();

        //選択肢後の分岐のデータを格納して後でadvDataに挿入する
        List<ADVData> choiceData = new List<ADVData>();

        bool isSceneEnd = false;

        for (int i = 0; i < lines.Length - 1; i++)
        {
            ADVData advDataTmp = new ADVData();

            do
            {
                //カンマ分けされたデータを格納
                //１行目はインデックス名なので読み込まない
                didCommaSeparationData = readCsv.DataSeparation(lines[i + 1], CSVDATA_ELEMENTS);

                //その行が何のデータなのかを調べる
                storeCommand(advDataTmp, didCommaSeparationData);

                if (advDataTmp.command == "sceneEnd")
                {
                    isSceneEnd = true;
                    break;
                }

                if (advDataTmp.command == "return")
                    break;

                if (advDataTmp.command == "minigame")
                {
                    advDataTmp.parameter = "minigame";
                    break;
                }

                if (isEventMode == false)
                    storeDrawData(advDataTmp, didCommaSeparationData);

                if (advDataTmp.command == "draw")
                    i++;

                if (advDataTmp.command == "choice")
                {
                    advDataTmp.choiceText[advDataTmp.choiceNum] = Convert.ToString(didCommaSeparationData[(int)ElementsName.TEXT]);
                    advDataTmp.choicePoint[advDataTmp.choiceNum] = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.PARAMETER]);
                    advDataTmp.choiceTermsParameter[advDataTmp.choiceNum] = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.CHARACTER_NAME]);
                    choiceTiming.Add(advDataTmp.choiceText[advDataTmp.choiceNum], advData.Count);
                    advDataTmp.choiceNum++;
                    i++;
                }
            } while (CheckCommand(advDataTmp.command) == false);

            //if (isEventMode == false && didCommaSeparationData[(int)ElementsName.BACK_GROUND] != "")
                //advDataTmp.backGroundID = BackgroundTextureNameToID(didCommaSeparationData[(int)ElementsName.BACK_GROUND]);

            if (advDataTmp.command == "send")
            {
                //テキスト表示用データの格納
                storeTextData(advDataTmp, didCommaSeparationData);
            }

            //音用のデータ格納
            //if (didCommaSeparationData[(int)ElementsName.BGM] != "")
                //advDataTmp.BGMID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BGM]);

            //コマンドが"sceneEnd"の行以降だったら
            if (isSceneEnd == false)
            {
                advData.Add(advDataTmp);
            }
            else
            {
                if (advDataTmp.command == "return")
                {
                    string key = choiceData[0].parameter;
                    choiceData[choiceData.Count - 1].parameter = (choiceTiming[key]).ToString();
                    choiceADVData.Add(key, choiceData);
                    choiceData = new List<ADVData>();
                }
                else if (advDataTmp.command != "sceneEnd")
                {
                    choiceData.Add(advDataTmp);
                }
                else
                {
                    advData.Add(advDataTmp);
                }
            }
        }
        /*
        foreach(ADVData d in advData)
        {
            Debug.Log(d.command + d.parameter + d.text);
        }*/
    }

    bool CheckCommand(string com)
    {
        string[] checkList = {"send","back","fadein","fadeout","fade" };
        foreach(string checkword in checkList)
        {
            if (com == checkword) return true;
        }
        return false;
    }

    public void SetMasteringCharacterLastStoryID()
    {
        if (DataManager.Instance.masteringData.masteringCharacterID != -1)
        {
            DataManager.Instance.masteringData.masteringCharacterLastChapterID = csvFile[DataManager.Instance.masteringData.masteringCharacterID].chapters.Length - 1;
            DataManager.Instance.masteringData.masteringCharacterLastStoryID = csvFile[DataManager.Instance.masteringData.masteringCharacterID].chapters[DataManager.Instance.nowReadChapterID].StoryText.Length - 1;
        }
    }

    void storeCommand(ADVData csv_, string[] didCommaSeparationData_)
    {
        if (didCommaSeparationData_[(int)ElementsName.COMMANDO] != "")
        {
            csv_.command = Convert.ToString(didCommaSeparationData_[(int)ElementsName.COMMANDO]);
        }

        if (didCommaSeparationData_[(int)ElementsName.PARAMETER] != "")
        {
            csv_.parameter = didCommaSeparationData_[(int)ElementsName.PARAMETER];
        }
    }

    void storeDrawData(ADVData csv_, string[] didCommaSeparationData_)
    {
        string drawCharacterName = didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME];
        if (CharacterNameToID(didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME]) < 0)
        {
            drawCharacterName = "";
        }
        
        //描画用データの格納            
        if (didCommaSeparationData_[(int)ElementsName.EXPRESSION] != "")
            csv_.expression[csv_.drawCharacterNum] = ExpressionToID(didCommaSeparationData_[(int)ElementsName.EXPRESSION]);

        if (didCommaSeparationData_[(int)ElementsName.COSTUME] != "")
            csv_.costume[csv_.drawCharacterNum] = CostumeToID(didCommaSeparationData_[(int)ElementsName.COSTUME]);
        //csv_.costume[csv_.drawCharacterNum] = Convert.ToUInt16(didCommaSeparationData_[(int)ElementsName.COSTUME]);

        if (didCommaSeparationData_[(int)ElementsName.POSITION_X] != "" /*&&
            didCommaSeparationData_[(int)ElementsName.POSITION_Y] != ""*/)
            csv_.pos[csv_.drawCharacterNum] = new Vector2(
                positionDataDictionary[didCommaSeparationData_[(int)ElementsName.POSITION_X]],
                //PositionNameToPosition(didCommaSeparationData_[(int)ElementsName.POSITION_X]),
                //Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.POSITION_X]),
                //Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.POSITION_Y])
                (sizeDataDictionary[didCommaSeparationData_[(int)ElementsName.SIZE]]-0.5f)*-600);

        if (didCommaSeparationData_[(int)ElementsName.SIZE] != "")
            csv_.size[csv_.drawCharacterNum] = sizeDataDictionary[didCommaSeparationData_[(int)ElementsName.SIZE]];
        //SizeNameToSize(didCommaSeparationData_[(int)ElementsName.SIZE]);
        //csv_.size[csv_.drawCharacterNum] = Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.SIZE]);
        if (drawCharacterName != "")
        {
            csv_.drawCharacterID[csv_.drawCharacterNum] = CharacterNameToID(/*didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME]*/drawCharacterName);
            csv_.drawCharacterNum++;
        }
    }

    void storeTextData(ADVData csv_, string[] didCommaSeparationData_)
    {
        //テキスト表示用データの格納
        if (didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME] != "")
            csv_.sendCharacter = Convert.ToString(didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME]);

        if (didCommaSeparationData_[(int)ElementsName.TEXT] != "")
        {
            string text = ConvertNewLineCode(didCommaSeparationData_[(int)ElementsName.TEXT]);
            //文章の前後にあるダブルクオートを消す
            csv_.text = text.Trim('"');
        }
    }

    /// <summary>
    /// 文字列内の"\n"の文字列を'\n'に文字に変える
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    string ConvertNewLineCode(string str)
    {
        return str.Replace("\\" + "n", "\n");
    }

    /// <summary> キャラクター名から各キャラクターに割り振られているIDに変換 </summary>
    /// <param name="characterName"> キャラクター名 </param>
    /// <returns>キャラクターID</returns>
    int CharacterNameToID(string characterName_)
    {
        int id = -1;
        if (characterName_ == "辰己")
        {
            id = 0;
        }
        if (characterName_ == "酉助")
        {
            id = 1;
        }
        return id;
    }

    /// <summary> 表情名から各表情に割り振られているIDに変換 </summary>
    /// <param name="characterName"> 表情名 </param>
    /// <returns>表情ID</returns>
    int ExpressionToID(string expression_)
    {
        int id = 0;
        if (expression_ == "通常")
        {
            id = 0;
        }
        if (expression_ == "喜")
        {
            id = 1;
        }
        if (expression_ == "怒")
        {
            id = 2;
        }
        if (expression_ == "哀")
        {
            id = 3;
        }
        if (expression_ == "楽")
        {
            id = 4;
        }
        return id;
    }

    /// <summary> 服装名から各服装に割り振られているIDに変換 </summary>
    /// <param name="costume_">服装名</param>
    /// <returns>服装ID</returns>
    int CostumeToID(string costume_)
    {
        int id = 0;
        if (costume_ == "制服")
        {
            id = 0;
        }
        return id;
    }

    /// <summary> 背景名から各背景に割り振られているIDに変換 </summary>
    /// <param name="backgroundTextureName_">背景名</param>
    /// <returns>背景に割り振られたID</returns>
    int BackgroundTextureNameToID(string backgroundTextureName_)
    {
        int id = 0;
        if (backgroundTextureName_ == "白")
        {
            id = 0;
        }
        if (backgroundTextureName_ == "黒")
        {
            id = 1;
        }
        if (backgroundTextureName_ == "町")
        {
            id = 2;
        }
        if (backgroundTextureName_ == "部屋")
        {
            id = 3;
        }
        if (backgroundTextureName_ == "火事")
        {
            id = 4;
        }
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary> デバッグ用 </summary>
/*    private const int LOG_MAX = 10;
    private Queue<string> logStack = new Queue<string>(LOG_MAX);


    /// <summary>
    /// ログを取得するコールバック
    /// </summary>
    /// <param name="condition">メッセージ</param>
    /// <param name="stackTrace">コールスタック</param>
    /// <param name="type">ログの種類</param>
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        // 通常ログまで表示すると邪魔なので無視
        if (type == LogType.Log)
            return;

        string trace = null;
        string color = null;

        switch (type)
        {
            case LogType.Warning:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "yellow";
                break;
            case LogType.Error:
            case LogType.Assert:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "red";
                break;
            case LogType.Exception:
                trace = stackTrace;
                color = "red";
                break;
        }

        // ログの行制限
        if (this.logStack.Count == LOG_MAX)
            this.logStack.Dequeue();

        string message = string.Format("<color={0}>{1}</color>", color, condition);
        this.logStack.Enqueue(message);
    }

    /// <summary>
    /// エラーログ表示
    /// </summary>
    void OnGUI()
    {
        if (this.logStack == null || this.logStack.Count == 0)
            return;

        // 表示領域は任意
        float space = 16f;
        float height = 500f;
        Rect drawArea = new Rect(space, 0 + space, (float)Screen.width * 0.5f, height);
        GUI.Box(drawArea, "");

        GUILayout.BeginArea(drawArea);
        {
            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            foreach (string log in logStack)
                GUILayout.Label(log, style);
        }
        GUILayout.EndArea();
    }*/
}