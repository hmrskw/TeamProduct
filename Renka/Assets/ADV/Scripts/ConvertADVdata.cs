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

        public string sendCharacter;//キャラクターのID
        public string text;//テキスト

        public int backGroundID;//背景のID
        public int BGMID;//鳴らすBGMのID

        public ADVData()
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
            this.choiceText[0] = "";
            this.choicePoint[0] = 0;

            this.sendCharacter = "";
            this.text = "";

            this.backGroundID = 0;
            this.BGMID = 0;
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
            this.choiceText[0] = "";
            this.choicePoint[0] = 0;

            this.sendCharacter = "";
            this.text = "";

            this.backGroundID = 0;
            this.BGMID = 0;
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
        */

        PARAMETER,//コマンドで必要なパラメーターの入力欄

        CHARACTER_NAME,//[draw]描画するキャラクターの名前[send]話しているキャラクターの名前

        EXPRESSION,//表情
        COSTUME,//服装
        POSITION_X,//キャラクター描画位置の横
        POSITION_Y,//キャラクター描画位置の縦
        SIZE,//キャラクター描画サイズ

        TEXT,//表示するテキスト

        BACK_GROUND,//変更する背景
        BGM,//鳴らすBGM
    }

    //csvデータの要素数
    const int CSVDATA_ELEMENTS = 11;

    //csvから取り出した情報を入れる配列
    public List<ADVData> advData;

    bool isEventMode = false;

    public List<ADVData> AdvData
    {
        get { return advData; }
    }

    //選択肢での分岐部分のデータ
    public Dictionary<string, List<ADVData>> choiceADVData;

    //パスの名前
    private string[] pathName =
    {
        "story01.csv", //１話
    };

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        readCsv = new ReadCSV();

        string[] lines = readCsv.ReadFile("story01.csv");
        
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

                if (isEventMode == false)
                    storeDrawData(advDataTmp, didCommaSeparationData);

                if (advDataTmp.command == "draw")
                    i++;

                if (advDataTmp.command == "choice")
                {
                    advDataTmp.choiceText[advDataTmp.choiceNum] = Convert.ToString(didCommaSeparationData[(int)ElementsName.TEXT]);
                    advDataTmp.choicePoint[advDataTmp.choiceNum] = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.PARAMETER]);
                    choiceTiming.Add(advDataTmp.choiceText[advDataTmp.choiceNum], advData.Count);
                    advDataTmp.choiceNum++;
                    i++;
                }
            } while (advDataTmp.command != "send");

            if (isEventMode == false && didCommaSeparationData[(int)ElementsName.BACK_GROUND] != "")
                advDataTmp.backGroundID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BACK_GROUND]);

            //テキスト表示用データの格納
            storeTextData(advDataTmp, didCommaSeparationData);

            //音用のデータ格納
            if (didCommaSeparationData[(int)ElementsName.BGM] != "")
                advDataTmp.BGMID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BGM]);

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
            }
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
        //描画用データの格納            
        if (didCommaSeparationData_[(int)ElementsName.EXPRESSION] != "")
            csv_.expression[csv_.drawCharacterNum] = ExpressionToID(didCommaSeparationData_[(int)ElementsName.EXPRESSION]);

        if (didCommaSeparationData_[(int)ElementsName.COSTUME] != "")
            csv_.costume[csv_.drawCharacterNum] = Convert.ToUInt16(didCommaSeparationData_[(int)ElementsName.COSTUME]);

        if (didCommaSeparationData_[(int)ElementsName.POSITION_X] != "" &&
            didCommaSeparationData_[(int)ElementsName.POSITION_Y] != "")
            csv_.pos[csv_.drawCharacterNum] = new Vector2(
                Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.POSITION_X]),
                Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.POSITION_Y]));

        if (didCommaSeparationData_[(int)ElementsName.SIZE] != "")
            csv_.size[csv_.drawCharacterNum] = Convert.ToSingle(didCommaSeparationData_[(int)ElementsName.SIZE]);
        if (didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME] != "")
        {
            csv_.drawCharacterID[csv_.drawCharacterNum] = CharacterNameToID(didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME]);
            csv_.drawCharacterNum++;
        }
    }

    void storeTextData(ADVData csv_, string[] didCommaSeparationData_)
    {
        //テキスト表示用データの格納
        if (didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME] != "")
            csv_.sendCharacter = Convert.ToString(didCommaSeparationData_[(int)ElementsName.CHARACTER_NAME]);

        if (didCommaSeparationData_[(int)ElementsName.TEXT] != "")
            csv_.text = Convert.ToString(didCommaSeparationData_[(int)ElementsName.TEXT]);
    }

    /// <summary> キャラクター名から各キャラクターに割り振られているIDに変換 </summary>
    /// <param name="characterName"> キャラクター名 </param>
    int CharacterNameToID(string characterName_)
    {
        int id = 0;
        if (characterName_ == "村人A")
        {
            id = 0;
        }
        if (characterName_ == "村人B")
        {
            id = 1;
        }
        return id;
    }

    /// <summary> キャラクター名から各キャラクターに割り振られているIDに変換 </summary>
    /// <param name="characterName"> キャラクター名 </param>
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
}
