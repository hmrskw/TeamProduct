using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine.Assertions;

public class ReadCSV
{
    public struct CSVData
    {
        public string command;//コマンド
        public float parameter;//コマンドに使うパラメーター

        public int drawCharacterID;//キャラクターのID
        public int expression;//表情
        public int costume;//服装
        public Vector2 pos;//表示位置
        public float size;//表示サイズ

        public string sendCharacter;//キャラクターのID
        public string text;//テキスト

        public int backGroundID;//背景のID
        public int BGMID;//鳴らすBGMのID
    }

    enum ElementsName
    {
        COMMANDO,
        /*
        [send]
            キャラクターのテキスト表示
        [draw]
            キャラクターの描画のみ
        [reset]
            描画しているキャラクターの非表示
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
    private CSVData[] csvData;

    public CSVData[] CsvData
    {
        get { return csvData; }
    }

    //パスの名前
    private string[] pathName =
    {
        "story01.csv", //１話
    };

    /// <summary>
    /// ファイルを読み込んでCSVデータに格納
    /// </summary>
    public void ReadFile()
    {
        int suffixNumber = UnityEngine.Random.Range(0, pathName.Length);

        //呼んでくるCSVファイルのパスを生成
        string path = Application.dataPath + "/CSVFiles/" + pathName[suffixNumber];

        //CSVデータを読み込んで、行に分割
        string[] lines = ReadCsvData(path);

        //csvデータの初期化
        csvData = new CSVData[lines.Length];

        //カンマ分けされたデータを仮格納する。その初期化
        string[] didCommaSeparationData = new string[lines.Length];

        //CSVデータを区切る文字
        char[] commaSpliter = { ',' };

        for (int i = 0; i < lines.Length-1; i++)
        {
            //カンマ分けされたデータを格納
            didCommaSeparationData = DataSeparation(lines[i+1], commaSpliter, CSVDATA_ELEMENTS);

            //データが空じゃなければ最適な型に変換してcsvDataに格納
            if (didCommaSeparationData[(int)ElementsName.COMMANDO] != "")
                csvData[i].command = Convert.ToString(didCommaSeparationData[(int)ElementsName.COMMANDO]);

            if (didCommaSeparationData[(int)ElementsName.PARAMETER] != "")
                csvData[i].parameter = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.PARAMETER]);
            
            if (didCommaSeparationData[(int)ElementsName.CHARACTER_NAME] != "")
               csvData[i].drawCharacterID = CharacterNameToID(didCommaSeparationData[(int)ElementsName.CHARACTER_NAME]);

            if (didCommaSeparationData[(int)ElementsName.EXPRESSION] != "")
                csvData[i].expression = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.EXPRESSION]);

            if (didCommaSeparationData[(int)ElementsName.COSTUME] != "")
                csvData[i].costume = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.COSTUME]);

            if (didCommaSeparationData[(int)ElementsName.POSITION_X] != "" &&
                didCommaSeparationData[(int)ElementsName.POSITION_Y] != "")
                csvData[i].pos = new Vector2( 
                    Convert.ToSingle(didCommaSeparationData[(int)ElementsName.POSITION_X]), 
                    Convert.ToSingle(didCommaSeparationData[(int)ElementsName.POSITION_X]));

            if (didCommaSeparationData[(int)ElementsName.SIZE] != "")
                csvData[i].size = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.SIZE]);

            if (didCommaSeparationData[(int)ElementsName.CHARACTER_NAME] != "")
                csvData[i].sendCharacter = Convert.ToString(didCommaSeparationData[(int)ElementsName.CHARACTER_NAME]);

            if (didCommaSeparationData[(int)ElementsName.TEXT] != "")
                csvData[i].text = Convert.ToString(didCommaSeparationData[(int)ElementsName.TEXT]);

            if (didCommaSeparationData[(int)ElementsName.BACK_GROUND] != "")
                csvData[i].backGroundID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BACK_GROUND]);

            if (didCommaSeparationData[(int)ElementsName.BGM] != "")
                csvData[i].BGMID = Convert.ToUInt16(didCommaSeparationData[(int)ElementsName.BGM]);
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
        if(characterName_ == "村人A")
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