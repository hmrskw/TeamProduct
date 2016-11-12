using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextManager : MonoBehaviour
{

    //ゲージなどの最大値が決まった値を扱うクラス
    public class IntGage
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="val">最大値の設定</param>
        public IntGage(int val) { max = val; num = 0; }

        //ゲージの最大
        public int max;

        //実際値
        public int num;

        /// <summary>
        /// /実際値が最大かどうか
        /// </summary>
        /// <returns>最大時 : true</returns>
        public bool CheckMax()
        {
            if (num >= max) return true;
            return false;
        }

        /// <summary>
        /// 実際値に加算
        /// </summary>
        /// <param name="val">加算値、デフォルトの場合は1加算</param>
        /// <returns>最大値の時 : true</returns>
        public bool Add(int val = 1)
        {
            num += val;
            if (num >= max)
            {
                num = max;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 実際値を0にリセットする
        /// </summary>
        /// <param name="val">引数をとる場合、最大値を変更</param>
        public void Reset(int val = -1)
        {
            if (val != -1) max = val;
            num = 0;
        }

        /// <summary>
        ///実際値を最大に
        /// </summary>
        public void AddNumMax() { num = max; }
    }

    [SerializeField, Tooltip("名前を表示するためのテキストボックス")]
    Text nameText;

    [SerializeField, Tooltip("本文を表示するためのテキストボックス")]
    Text text;

    [SerializeField,Tooltip("インターミッションのテキストボックス")]
    Text intermissionText;

    [SerializeField, Range(0, 5), Tooltip("このフレーム分待機する")]
    int wait;

    //セリフを表示するときのキャラクターの情報
    [System.Serializable]
    public struct Charctor
    {
        //キャラクターの番号
        public int number;

        //キャラクターの座標、z値で大きさ
        public Vector3 pos;
    }

    /*
    //セリフ...words
    [System.Serializable]
    public class Words
    {
        //表示する一言分の会話
        public string word;

        //背景の番号
        public int backgroundNumber;

        //キャラクター　任意数
        public Charctor[] charactor;
    }

    //対話....Dialogue
    //[SerializeField]
    //Words[] dialogue;
    */

    //[SerializeField]
    //ChoiceManager choiceManager;

    //一つのセリフの文字列の文字
    IntGage stringCount = new IntGage(0);

    //文字一つ一つの間隔
    IntGage charIntervalCount = new IntGage(0);

    public List<ConvertADVdata.ADVData> nowRead { set; private get; }


    [SerializeField, Tooltip("一行の文字列幅")]
    int strLength;

    [SerializeField, Tooltip("禁則文字")]
    char[] prohibitionCharacters;

    void Start()
    {

        nameText.text = "";
        text.text = "";

        nowRead = ConvertADVdata.Instance.AdvData;

        stringCount.Reset(nowRead[DataManager.Instance.endLine].text.Length);
        charIntervalCount.Reset(wait);

        StartCoroutine(ADVUpdate());
    }

    public void ShiftNextText()
    {
        if ((/*ReadCSV.Instance.csvData*/nowRead.Count > DataManager.Instance.endLine))
        {
            DataManager.Instance.endLine++;
            //カウンターのリセット
            //Debug.Log(DataManager.Instance.endLine);
            //Debug.Log(nowReadCSV[DataManager.Instance.endLine].text);
            stringCount.Reset(/*ReadCSV.Instance.CsvData*/nowRead[DataManager.Instance.endLine].text.Length);
        }
    }

    public void DrawAllText()
    {
        stringCount.AddNumMax();

        //キャラクター名を表示
        nameText.text = /*ReadCSV.Instance.CsvData*/nowRead[DataManager.Instance.endLine].sendCharacter;

        //一文すべてを表示する
        //text.text = /*ReadCSV.Instance.CsvData*/nowRead[DataManager.Instance.endLine].text;
        text.text = ConvertJpHyph(nowRead[DataManager.Instance.endLine].text, strLength);
    }

    public bool IsDrawAllText()
    {
        return stringCount.CheckMax();
    }

    /// <summary>
    /// ADVのアップデート
    /// </summary>
    /// <returns></returns>
    IEnumerator ADVUpdate()
    {
        while (true)
        {
            //文字送り中の時
            if (!stringCount.CheckMax())
            {
                //文字の表示間隔カウントが最大でないとき
                if (!charIntervalCount.CheckMax())
                {
                    //文字の表示間隔カウントを１足す
                    charIntervalCount.Add();
                }
                else
                {
                    //文に文字数カウントを足す
                    stringCount.Add();

                    //文字表示間隔カウントのリセット
                    charIntervalCount.Reset();

                    //キャラクター名を表示
                    nameText.text = /*ReadCSV.Instance.CsvData*/nowRead[DataManager.Instance.endLine].sendCharacter;

                    //文の文字を足す
                    //text.text = /*ReadCSV.Instance.CsvData*/nowRead[DataManager.Instance.endLine].text.Substring(0, stringCount.num);
                    //Debug.Log( "el" + DataManager.Instance.endLine + "\nstringCount.num" + stringCount.num);
                    //Debug.Log( nowRead[DataManager.Instance.endLine].text.Substring(0, stringCount.num) );
                    text.text = ConvertJpHyph(nowRead[DataManager.Instance.endLine].text.Substring(0, stringCount.num) ,strLength );
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 禁則文字かどうか
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    bool IsProhiChar(char c)
    {
        for (int i = 0; i < prohibitionCharacters.Length; i++)
        {
            if (c == prohibitionCharacters[i]) return true;
        }

        return false;
    }

    /// <summary>
    /// 禁則処理(全角のみ)
    /// </summary>
    /// <param name="str">変化する文字列</param>
    /// <param name="length">文字幅</param>
    /// <returns>禁則処理で変換された文字列</returns>
    /// ※UIの表示範囲外のlengthをしてすると発狂します
    string ConvertJpHyph(string str, int length)
    {
        //Debug.Log("StrLength : " + str.Length);
        //Debug.Log("Length : " + length);

        if (length < 1)
        {
            length = 1;
        }

        string buffer = "";
        int strCount = 0;

        while (strCount < str.Length)
        {
            //Debug.Log("str count : " + strCount);

            //改行したかどうか
            bool endl = false;

            //1.行の長さごとに改行コードないか調べて
            //2.行の長さを計算する
            int i = 0;
            for (i = 0; i < length; ++i)
            {
                //Debug.Log("i count : " + i);
                //文字列の長さを超えるか
                if (strCount + i >= str.Length - 1)
                {
                    //残りをバッファに追加する
                    buffer += str.Substring(strCount, i + 1);
                    //Debug.Log("return 1");
                    return buffer;
                }

                //次の先頭文字\nなら改行させるために小ループを抜ける
                char target = str[strCount + i];
                if (target == '\n')
                {
                    ++i;
                    endl = true;
                    //Debug.Log("\\n");
                    break;
                }

            }

            //計算した長さ文だけの文字列を加える
            buffer += str.Substring(strCount, i);
            //Debug.Log("buffer += " + str.Substring(strCount, i + 1));
            //Debug.Log("StrCount += " + i);
            strCount += i;

            //禁則処理
            if (strCount < str.Length && IsProhiChar(str[strCount]))
            {
                //Debug.Log("禁則処理");
                buffer += str[strCount];
                ++strCount;
                endl = true;

            }

            if (endl == false && str[strCount] != '\n') buffer += '\n';
        }

        //Debug.Log("return 2");
        return buffer;
    }

    public void IntermissionTextChange()
    {
        intermissionText.text = DataManager.Instance.nowReadStoryID + "話　プロット";
    }
    /*
    /// <summary>
    /// 文字列内の"\n"の文字列を'\n'に文字に変える
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    string ConvertNewLineCode(string str)
    {
        return str.Replace("\\" + "n", "\n");
    }*/
}