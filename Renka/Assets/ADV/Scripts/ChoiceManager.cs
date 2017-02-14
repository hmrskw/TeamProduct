using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//グラフィックマネージャーが整理されるまで一時的に分けておく
public class ChoiceManager : MonoBehaviour
{
    [SerializeField, Tooltip("選択肢のプレハブ")]
    GameObject choicePrefabObj;

    [SerializeField, Tooltip("生成先")]
    GameObject ChoicesObject;

    //選択肢オブジェクトの生成数
    [SerializeField, Range(1, 3), Tooltip("選択肢の最大数")]
    int choiceObjNum;

    //選択肢オブジェクトへの参照用
    Choice[] choiceScripts;

    //選択肢がでて、ボタンを押せる状態か
    public bool isActiveChoices { get; private set; }

    [SerializeField, Tooltip("選択肢の出現座標")]
    Vector3 choicePos;

    [SerializeField, Tooltip("選択ごとの幅")]
    float choiceWidth;

    [SerializeField]
    GameObject checkPopup;

    [SerializeField]
    ChoiceCheck choiceCheck;

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

    /// <summary>
    /// 選択肢
    /// </summary>
    public string Choice()
    {
        if (choiceCheck.isPassingCheck == false) return null;
        //選択肢を押したあと離した時
        //for (int i = 0; i < choiceScripts.Length; ++i)
        {
            //if (choiceScripts[i].isReleased == false) continue;
            if (SceneChanger.GetBeforeSceneName() != "Menu")
            {
                DataManager.Instance.masteringData.likeabillity += choiceCheck.Point;
            }
            Debug.Log("Add好感度 : " + DataManager.Instance.masteringData.likeabillity);
            string choiceText = choiceCheck.Text;
            DataManager.Instance.isChoiceText = true;

            //選択肢を消す
            for (int k = 0; k < choiceScripts.Length; ++k)
            {
                choiceScripts[k].gameObject.SetActive(false);
                //choiceScripts[k].Reset();
            }

            isActiveChoices = false;
            choiceCheck.isPassingCheck = false;
            return choiceText;
            //break;
        }
    }

    /// <summary>
    /// 選択肢を描画する
    /// </summary>
    /// <param name="wordsCount">CSVの何行目を読み込むか</param>
    public void DrawChoice(int wordsCount)
    {
        //0番目の選択を出現させる
        if (ConvertADVData.Instance.AdvData[wordsCount].choiceNum != 0)
        {
            //選択肢から選択の数を取得
            int num = ConvertADVData.Instance.AdvData[wordsCount].choiceNum;//choicesArray[0].choies.Length;
            for (int i = 0; i < num; ++i)
            {
                //選択肢の配列が小さい場合
                if (i > choiceScripts.Length - 1) break;

                //選択肢が選ばれたときに入るポイントをコピー
                choiceScripts[i].point = ConvertADVData.Instance.AdvData[wordsCount].choicePoint[i];//choicesArray[0].choies[i].point;
                //選択のテキストをオブジェにコピー
                choiceScripts[i].text.text = ConvertADVData.Instance.AdvData[wordsCount].choiceText[i];//choicesArray[0].choies[i].str;
                //選択肢を選んだ後に表示されるポップアップの参照をコピー
                choiceScripts[i].checkPopup = checkPopup;

                //選択の座標計算
                //未実装

                //選択のオブジェをアクティブする
                //if (ConvertADVData.Instance.AdvData[wordsCount].choiceTermsParameter[i] > DataManager.Instance.masteringData.likeabillity)
                //{
                    //choiceScripts[i].SetCanPush(false);
                //}

                choiceScripts[i].gameObject.SetActive(true);

                //choiceScripts[i].Reset();
            }

            //選択肢をアクティブに
            isActiveChoices = true;

        }
    }
}