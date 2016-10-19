using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine.Assertions;

public class ReadCSV : MonoBehaviour
{
    private static ReadCSV instance;

    public static ReadCSV Instance
    {
        get { return instance; }
    }

    public class CSVData
    {
        public string command;//コマンド
        public float parameter;//コマンドに使うパラメーター

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

        public CSVData()
        {
            this.command = "";
            this.parameter = 0f;

            this.drawCharacterNum = 0;
            this.drawCharacterID[0] = 0;
            this.expression[0] = 0;
            this.costume[0] = 0;
            this.pos[0] = Vector2.zero;
            this.size[0] = 0f;

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
            this.parameter = 0f;

            this.drawCharacterNum = 0;
            this.drawCharacterID[0] = 0;
            this.expression[0] = 0;
            this.costume[0] = 0;
            this.pos[0] = Vector2.zero;
            this.size[0] = 0f;

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
    public List<CSVData> csvData;

    bool isEventMode = false;

    public List<CSVData> CsvData
    {
        get { return csvData; }
    }

    //パスの名前
    private string[] pathName =
    {
        "story01.csv", //１話
    };

    //シーンまたいでもオブジェクトが破棄されなくする
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        ReadFile();
    }

    /// <summary>
    /// ファイルを読み込んでCSVデータに格納
    /// </summary>
    void ReadFile()
    {
        int suffixNumber = UnityEngine.Random.Range(0, pathName.Length);

        //呼んでくるCSVファイルのパスを生成
        string path = Application.dataPath + "/CSVFiles/" + pathName[suffixNumber];

        //CSVデータを読み込んで、行に分割
        string[] lines = ReadCsvData(path);

        //csvデータの初期化
        csvData = new List<CSVData>();

        //カンマ分けされたデータを仮格納する。その初期化
        string[] didCommaSeparationData = new string[lines.Length];

        //CSVデータを区切る文字
        char[] commaSpliter = { ',' };

        for (int i = 0; i < lines.Length - 1; i++)
        {
            CSVData csvDataTmp = new CSVData();

            do
            {
                //カンマ分けされたデータを格納
                //１行目はインデックス名なので読み込まない
                didCommaSeparationData = DataSeparation(lines[i + 1], commaSpliter, CSVDATA_ELEMENTS);

                //データが空じゃなければ最適な型に変換してcsvDataに格納
                //その行が何のデータなのかを調べる
                if (didCommaSeparationData[(int)ElementsName.COMMANDO] != "")
                {
                    csvDataTmp.command = Convert.ToString(didCommaSeparationData[(int)ElementsName.COMMANDO]);
                }

                if (didCommaSeparationData[(int)ElementsName.PARAMETER] != "")
                {
                    csvDataTmp.parameter = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.PARAMETER]);
                }

                if (isEventMode == false)
                {
                    //描画用データの格納            
                    if (didCommaSeparationData[(int)ElementsName.CHARACTER_NAME] != "")
                        csvDataTmp.drawCharacterID[csvDataTmp.drawCharacterNum] = CharacterNameToID(didCommaSeparationData[(int)ElementsName.CHARACTER_NAME]);

                    if (didCommaSeparationData[(int)ElementsName.EXPRESSION] != "")
                        csvDataTmp.expression[csvDataTmp.drawCharacterNum] = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.EXPRESSION]);

                    if (didCommaSeparationData[(int)ElementsName.COSTUME] != "")
                        csvDataTmp.costume[csvDataTmp.drawCharacterNum] = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.COSTUME]);

                    if (didCommaSeparationData[(int)ElementsName.POSITION_X] != "" &&
                        didCommaSeparationData[(int)ElementsName.POSITION_Y] != "")
                        csvDataTmp.pos[csvDataTmp.drawCharacterNum] = new Vector2(
                            Convert.ToSingle(didCommaSeparationData[(int)ElementsName.POSITION_X]),
                            Convert.ToSingle(didCommaSeparationData[(int)ElementsName.POSITION_X]));

                    if (didCommaSeparationData[(int)ElementsName.SIZE] != "")
                        csvDataTmp.size[csvDataTmp.drawCharacterNum] = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.SIZE]);

                    if (csvDataTmp.command == "draw")
                    {
                        csvDataTmp.drawCharacterNum++;
                        i++;
                    }
                }
                if (csvDataTmp.command == "choice")
                {
                    csvDataTmp.choiceText[csvDataTmp.choiceNum] = Convert.ToString(didCommaSeparationData[(int)ElementsName.TEXT]);
                    csvDataTmp.choicePoint[csvDataTmp.choiceNum] = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.PARAMETER]);
                    csvDataTmp.choiceNum++;
                    i++;
                }

            } while (csvDataTmp.command != "send");

            if (didCommaSeparationData[(int)ElementsName.BACK_GROUND] != "")
                csvDataTmp.backGroundID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BACK_GROUND]);

            //テキスト表示用データの格納
            if (didCommaSeparationData[(int)ElementsName.CHARACTER_NAME] != "")
                csvDataTmp.sendCharacter = Convert.ToString(didCommaSeparationData[(int)ElementsName.CHARACTER_NAME]);

            if (didCommaSeparationData[(int)ElementsName.TEXT] != "")
                csvDataTmp.text = Convert.ToString(didCommaSeparationData[(int)ElementsName.TEXT]);

            //音用のデータ格納
            if (didCommaSeparationData[(int)ElementsName.BGM] != "")
                csvDataTmp.BGMID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BGM]);

            csvData.Add(csvDataTmp);
            /*Debug.Log("command:" + csvDataTmp.command + ",parameter:" + csvDataTmp.parameter + ",drawCharacterNum:" + csvDataTmp.drawCharacterNum +
                "\ndrawCharacterID[0]:" + csvDataTmp.drawCharacterID[0] + ",expression[0]:" + csvDataTmp.expression[0] + ",costume[0]:" + csvDataTmp.costume[0] + ",pos[0]:" + csvDataTmp.pos[0] + ",size[0]:" + csvDataTmp.size[0] +
                "\ndrawCharacterID[1]:" + csvDataTmp.drawCharacterID[1] + ",expression[1]:" + csvDataTmp.expression[1] + ",costume[1]:" + csvDataTmp.costume[1] + ",pos[1]:" + csvDataTmp.pos[1] + ",size[1]:" + csvDataTmp.size[1] +
                "\ndrawCharacterID[1]:" + csvDataTmp.choicePoint[0] + ",expression[1]:" + csvDataTmp.expression[1] + ",costume[1]:" + csvDataTmp.costume[1] + ",pos[1]:" + csvDataTmp.pos[1] + ",size[1]:" + csvDataTmp.size[1] +
                "\nsendCharacter:" + csvDataTmp.sendCharacter + ",text:" + csvDataTmp.text + ",backGroundID:" + csvDataTmp.backGroundID + "BGMID:" + csvDataTmp.BGMID
                );*/
        }

        foreach (CSVData csv in csvData)
        {
            Debug.Log("command:" + csv.command + ",parameter:" + csv.parameter + ",drawCharacterNum:" + csv.drawCharacterNum +
                "\ndrawCharacterID[0]:" + csv.drawCharacterID[0] + ",expression[0]:" + csv.expression[0] + ",costume[0]:" + csv.costume[0] + ",pos[0]:" + csv.pos[0] + ",size[0]:" + csv.size[0] +
                "\ndrawCharacterID[1]:" + csv.drawCharacterID[1] + ",expression[1]:" + csv.expression[1] + ",costume[1]:" + csv.costume[1] + ",pos[1]:" + csv.pos[1] + ",size[1]:" + csv.size[1] +
                "\nchoicePoint[0]:" + csv.choicePoint[0] + "choiceText[0]:" + csv.choiceText[0] + 
                "\nchoicePoint[1]:" + csv.choicePoint[1] + "choiceText[1]:" + csv.choiceText[1] +
                "\nchoicePoint[2]:" + csv.choicePoint[2] + "choiceText[2]:" + csv.choiceText[2] +
                "\nsendCharacter:" + csv.sendCharacter + ",text:" + csv.text + ",backGroundID:" + csv.backGroundID + "BGMID:" + csv.BGMID
                );
        }
    }

    ///<summary> ファイルを読み込み、配列に1行ずつ格納 </summary>
    ///<param name="path_"> 読み込むCSVデータファイルのパス </param>
    string[] ReadCsvData(string path_)
    {
        //ファイル読み込み
        StreamReader sr = new StreamReader(path_);
        //stringに変換
        string strStream = sr.ReadToEnd();

        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        //行に分ける
        string[] lines = strStream.Split(new char[] { '\r', '\n' }, option);

        return lines;
    }

    /// <summary> コンマでデータを分割する </summary>
    /// <param name="lines_"> ReadCsvData関数で一行にされたデータ </param>
    /// <param name="spliter_"> 渡されたデータを区切る文字 </param>
    /// <param name="trialNumber_"> 第一引数のデータの要素数。for文の周回数 </param>
    string[] DataSeparation(string lines_, char[] spliter_, int trialNumber_)
    {
        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.None;

        //リターン値。カンマ分けしたデータを一行分格納する。
        string[] CommaSeparationData = new string[trialNumber_];

        for (int i = 0; i < trialNumber_; i++)
        {
            //１行にあるCsvDataの要素数分準備する
            string[] readStrData = new string[trialNumber_];
            //CsvDataを引数の文字で区切って1つずつ格納
            readStrData = lines_.Split(spliter_, option);
            //readStrDataをリターン値に格納
            CommaSeparationData[i] = readStrData[i];
        }

        return CommaSeparationData;
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
        if (expression_ == "無")
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