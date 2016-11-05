using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;

public class ReadCSV
{
    /// <summary>
    /// ファイルを読み込んでCSVデータに格納
    /// </summary>
    public string[] ReadFile(string pathName)
    {
        /*
        string pathName;

        if (DataManager.Instance.masteringCharacterID == -1){
            pathName = prologueCSV.name + ".csv";
        }
        else{
            pathName = csvFile[DataManager.Instance.masteringCharacterID].StoryText[DataManager.Instance.nowReadStoryID].name + ".csv";
        }
        */
        //呼んでくるCSVファイルのパスを生成
        string path = Application.dataPath + "/CSVFiles/" + pathName;
        //CSVデータを読み込んで、行に分割
        string[] lines = ReadCsvData(path);

        return lines;
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
    public string[] DataSeparation(string lines_, /*char[] spliter_,*/ int trialNumber_)
    {
        char[] spliter_ = { ',' };

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
}