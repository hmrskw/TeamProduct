using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextManager : MonoBehaviour
{

    //ゲージなどの最大値が決まった値を扱うクラス
    public class IntGage
    {
        public IntGage(int val) { max = val; num = 0; }

        //ゲージの最大
        public int max;

        //実際値
        public int num;

        //実際値が最大かどうか
        public bool CheckMax()
        {
            if (num >= max) return true;
            return false;
        }

        //実際値に加算
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

        //実際値を0にリセットする
        //引数をとる場合、最大値を変更
        public void Reset(int val = -1)
        {
            if (val != -1) max = val;
            num = 0;
        }

        //実際値を最大に
        public void AddNumMax() { num = max; }
    }

    [SerializeField,Tooltip("名前を表示するためのテキストボックス")]
    Text nameText;

    [SerializeField,Tooltip("本文を表示するためのテキストボックス")]
    Text text;

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
    [SerializeField]
    Words[] dialogue;

    //セリフの数
    IntGage wordsCount = new IntGage(0);

    //一つのセリフの文字列の文字
    IntGage stringCount = new IntGage(0);

    //文字一つ一つの間隔
    //IntGage charCount = new IntGage(0);
    IntGage charIntervalCount = new IntGage(0);

    ReadCSV readCSV;
    void Awake()
    {
        readCSV = new ReadCSV();
        readCSV.ReadFile();
    }

    void Start()
    {
        nameText.text = "";
        text.text = "";

        //CSVファイルの最初は読み込まないので-1
        wordsCount.Reset(/*dialogue.Length - 1*/readCSV.CsvData.Length-2);
        stringCount.Reset(/*dialogue[0].word.Length*/readCSV.CsvData[0].text.Length);
        charIntervalCount.Reset(wait);

        StartCoroutine(ADVUpdate());
    }

    IEnumerator ADVUpdate()
    {
        //float waitFrame = wait / 60f;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //文字送り表示中かどうか
                if (stringCount.CheckMax())
                {
                    //次の文章が無ければループを抜ける
                    if (wordsCount.CheckMax()) break;
                     
                    wordsCount.Add();
                    Debug.Log("word : " + wordsCount.num);
                    //カウンターのリセット
                    stringCount.Reset(/*dialogue[wordsCount.num].word.Length*/readCSV.CsvData[wordsCount.num].text.Length);
                    //text.text = words[wordCount.num].word.Substring(0, stringCount.num);
                }
                else
                {
                    stringCount.AddNumMax();
                    Debug.Log("すべて表示する");
                    nameText.text = readCSV.CsvData[wordsCount.num].sendCharacter;
                    text.text = readCSV.CsvData[wordsCount.num].text;
                }
            }

            if (!stringCount.CheckMax())
            {
                if (!charIntervalCount.CheckMax())
                {
                    charIntervalCount.Add();
                }
                else
                {
                    stringCount.Add();

                    charIntervalCount.Reset();
                    nameText.text = readCSV.CsvData[wordsCount.num].sendCharacter;
                    text.text = readCSV.CsvData[wordsCount.num].text.Substring(0, stringCount.num);
                }
            }

            //スキップがうまくいかないバグが出る
            //if (!stringCount.CheckMax())
            //{
            //    yield return new WaitForSeconds(waitFrame);

            //    stringCount.Add();

            //    text.text = words[wordCount.num].word.Substring(0, stringCount.num);
            //}

            yield return null;
        }
    }

    // Update is called once per frame
    /*	void Update ()
        {
            if (Input.GetMouseButtonDown(0) && !wordCount.CheckMax())
            {
                //文字送り表示中かどうか
                if (stringCount.CheckMax())
                {
                    Debug.Log( "word : " + wordCount.num);
                    wordCount.Add();
                    //カウンターのリセット
                    stringCount.Reset(words[wordCount.num].word.Length);
                    text.text = words[wordCount.num].word.Substring(0, stringCount.num);
                }
                else
                {

                    stringCount.AddMax();
                    Debug.Log("More");
                    charCount.Reset();
                    text.text = words[wordCount.num].word;
                }          
            }


            if (!stringCount.CheckMax())
            {
                if (!charCount.CheckMax())
                {
                    charCount.Add();
                }
                else
                {
                    stringCount.Add();

                    charCount.Reset();
                    text.text = words[wordCount.num].word.Substring(0, stringCount.num);
                }
            }


        }*/
}
