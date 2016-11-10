using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ADVManager : MonoBehaviour
{

    ChoiceManager choiceManager;

    TextManager textManager;

    GraphicManager graphicManager;

    string key;

    List<ConvertADVdata.ADVData> nowRead;

    [SerializeField,Range(1,5)]
    int skipSpeed;

    float skipTimer = 0;

    void Start()
    {
        nowRead = ConvertADVdata.Instance.advData;
        choiceManager = GetComponent<ChoiceManager>();
        textManager = GetComponent<TextManager>();
        graphicManager = GetComponent<GraphicManager>();

        if (SceneChanger.GetBeforeSceneName(true) == "MiniGame")
        {
            //ADVシーンを終了
            DataManager.Instance.endLine = 0;
            DataManager.Instance.nowReadStoryID++;

            ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
            //インターミッションに移動
            graphicManager.ChangeCanvasNext();
        }
    }

    void Update()
    {
        if (choiceManager.isActiveChoices)
        {
            //選択肢表示中ならキーを取得
            key = choiceManager.Choice();
            if (key != null)
            {
                //取得したキーから読むADVデータを切り替える
                nowRead = ConvertADVdata.Instance.choiceADVData[key];
                textManager.nowRead = nowRead;
                DataManager.Instance.endLine = -1;
                textManager.ShiftNextText();
            }
        }
        else
        {
            if (textManager.IsDrawAllText())
            {
                choiceManager.DrawChoice(DataManager.Instance.endLine);
            }
            if (InputManager.Instance.IsTouchBegan())
            {
                //文字送り表示中かどうか
                //文字送りが終わってるとき
                if (textManager.IsDrawAllText())
                {
                    DrawNext();
                }
                else
                {
                    textManager.DrawAllText();
                }
            }
            skipTimer += Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftControl)){
                textManager.DrawAllText();
                if (skipTimer > 0.5f / skipSpeed)
                {
                    skipTimer = 0;
                    DrawNext();
                }
            }
        }
    }

    void DrawNext()
    {
        //文章の最後まで表示し終わっているか
        if (nowRead.Count - 1 <= DataManager.Instance.endLine)
        {
            if (DataManager.Instance.isChoiceText)
            {
                //選択肢で分岐した後の文章なら共通文章に戻る
                DataManager.Instance.isChoiceText = false;
                DataManager.Instance.endLine = Convert.ToInt16(nowRead[DataManager.Instance.endLine].parameter);
                nowRead = ConvertADVdata.Instance.advData;
                textManager.nowRead = nowRead;
                textManager.ShiftNextText();
            }
            else
            {
                if (DataManager.Instance.nowReadStoryID == 0)
                {
                    //ADVシーンを終了
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    //読んでいたシーンがプロローグなら攻略キャラ選択へ移動
                    SceneChanger.LoadScene("CharacterChoice");
                }
                else if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
                {
                    //パラメータがミニゲームだったらMiniGameシーンへ
                    SceneChanger.LoadScene("MiniGame", true);
                }
                else
                {
                    //ADVシーンを終了
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
                    //インターミッションに移動
                    graphicManager.ChangeCanvasNext();
                }
            }
        }
        else
        {
            if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
            {
                DataManager.Instance.endLine++;
                //パラメータがミニゲームだったらMiniGameシーンへ
                SceneChanger.LoadScene("MiniGame", true);
            }
            else
            {
                graphicManager.Reset();
                textManager.ShiftNextText();
                graphicManager.DrawCharacter(nowRead);
                graphicManager.DrawBack(nowRead[DataManager.Instance.endLine].backGroundID);
            }
        }
    }
}
