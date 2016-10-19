using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//グラフィックマネージャーが整理されるまで一時的に分けておく
public class ChoiceManager : MonoBehaviour
{
    // 選択肢の選択のデータ
    [System.Serializable]
    public struct Choice_
    {
        //何番目の選択
        public int order;

        //選択肢の文字列
        public string str;

        //選択できるか
        public bool isAbleSelects;

        //ポイント
        public int point;
        
    }

 
    // 選択肢、選択を格納しておく
    [System.Serializable]
    public class Choices_
    {
        public Choice_[] choies;

        void Setup(int choiceNum = 3)
        {
            choies = new Choice_[choiceNum];
        }

        void SetChoice(int idx, Choice_ obj)
        {
            choies[idx] = obj;
        }

    }

    [SerializeField, Tooltip("すべての選択肢の設定")]
    Choices_[] choicesArray;

    
    [SerializeField, Tooltip("選択肢のプレハブ")]
    GameObject choicePrefabObj;

    [SerializeField, Tooltip("生成先")]
    GameObject ChoicesObject;

    //選択肢オブジェクトの生成数
    [SerializeField, Range(1,3), Tooltip("選択肢の最大数")]
    int choiceObjNum;

    //選択肢オブジェクトへの参照用
    Choice[] choiceScripts;

    //選択肢がでて、ボタンを押せる状態か
    public bool isActiveChoices { get; private set; }

    //選択されたときのボタン番号 押されてないときは -1
    int selectNumber;

    [SerializeField, Tooltip("選択肢の出現座標")]
    Vector3 choicePos;

    [SerializeField, Tooltip("選択ごとの幅")]
    float choiceWidth;

    void Start()
    {          
        //参照用スクリプトを生成
        choiceScripts = new Choice[choiceObjNum];

        //選択肢オブジェクトの生成
        for (int i = 0; i < choiceObjNum; ++i)
        {
            var obj = (GameObject)Instantiate(choicePrefabObj, ChoicesObject.transform);
            choiceScripts[i] = obj.GetComponent<Choice>();
            choiceScripts[i].Setup();
            choiceScripts[i].transform.localScale = Vector3.one;
            choiceScripts[i].transform.localPosition = Vector3.zero;
            choiceScripts[i].gameObject.SetActive(false);
        }

        //座標計算
        Vector3 pos = choicePos; //一番目の座標
        for (int i = 0; i < choiceObjNum; ++i)
        {
            choiceScripts[i].transform.localPosition = pos;
            pos.y -= choiceWidth;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) Debug.Log(DataManager.Instance.likeabillity);
    }

    /// <summary>
    /// 選択肢
    /// </summary>
    public void Choice ()
    {
        //選択肢を押したあと離した時
        for (int i = 0; i < choiceScripts.Length; ++i)
        {
            if (!choiceScripts[i].isReleased) continue;

            Debug.Log("Add好感度 : " + DataManager.Instance.likeabillity);

            //選択肢を消す
            for (int k = 0; k < choiceScripts.Length; ++k)
            {
                choiceScripts[k].gameObject.SetActive(false);
                choiceScripts[k].Reset();
            }

            isActiveChoices = false;
            break;
        }
	}

    /// <summary>
    /// 選択肢を描画する
    /// </summary>
    /// <param name="wordsCount">CSVの何行目を読み込むか</param>
    public void DrawChoice(int wordsCount)
    {
        //0番目の選択を出現させる
        if (/*Input.GetKeyDown(KeyCode.Return)*/ReadCSV.Instance.CsvData[wordsCount].choiceNum != 0)
        {
            //選択肢から選択の数を取得
            int num = choicesArray[0].choies.Length;
            for (int i = 0; i < num; ++i)
            {
                //選択肢の配列が小さい場合
                if (i > choiceScripts.Length -1 ) break;

                //選択肢が選ばれたときに入るポイントをコピー
                choiceScripts[i].point = ReadCSV.Instance.CsvData[wordsCount].choicePoint[i];//choicesArray[0].choies[i].point;
                //選択のテキストをオブジェにコピー
                choiceScripts[i].text.text = ReadCSV.Instance.CsvData[wordsCount].choiceText[i];//choicesArray[0].choies[i].str;

                //選択の座標計算
                //未実装

                //選択のオブジェをアクティブする
                choiceScripts[i].gameObject.SetActive(true);

                choiceScripts[i].Reset();
            }

            //選択肢をアクティブに
            selectNumber = -1;
            isActiveChoices = true;

            //ログ
            //Debug.Log("好感度 : " + likeabillity );
        }
    }    
}

/*
 選択肢のオブジェクトは３つしか生成せず
    構造体でテキストとポイントを格納しておき
    選択時に参照させる

    ・選択肢の表示
    ・選択できる
    ・選択したら好感度を上げる

    座標を計算するもの
    選択肢の選択の数によって決まる


*/