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

    bool isIntermission = false;

    void Start()
    {
        nowRead = ConvertADVdata.Instance.advData;
        choiceManager = GetComponent<ChoiceManager>();
        textManager = GetComponent<TextManager>();
        graphicManager = GetComponent<GraphicManager>();
        if (SceneChanger.GetBeforeSceneName(true) != "Menu")
        {
            ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
        }
        //Debug.Log(DataManager.Instance.nowReadStoryID);
        if (SceneChanger.GetBeforeSceneName(true) == "MiniGame")
        {
            ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
            //インターミッションに移動
            isIntermission = true;
            textManager.IntermissionTextChange();
            graphicManager.ChangeCanvasNext();
            textManager.ChangeCanvasNext();
        }
    }

    void Update()
    {
        if (isIntermission == true) return;
        if (Fade.Instance.isFade == true) return;

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
        else if (nowRead[DataManager.Instance.endLine].command == "fadein")
        {
            Fade.Instance.FadeIn(0.5f, Convert.ToInt16(nowRead[DataManager.Instance.endLine].parameter));
            DrawNext();
        }
        else if (nowRead[DataManager.Instance.endLine].command == "fadeout")
        {
            Fade.Instance.FadeOut(0.5f, Convert.ToInt16(nowRead[DataManager.Instance.endLine].parameter));
            DrawNext();
        }
        else if (nowRead[DataManager.Instance.endLine].command == "back")
        {
            graphicManager.DrawBack(nowRead[DataManager.Instance.endLine].parameter);
            DrawNext();
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
            if (Input.GetKey(KeyCode.LeftControl))
            {
                textManager.DrawAllText();
                if (skipTimer > 0.5f / skipSpeed)
                {
                    skipTimer = 0;
                    DrawNext();
                }
            }
            //既読スキップ処理
            if (DataManager.Instance.nowReadStoryID < 4)
            {
                if (InputManager.Instance.TouchTime() > 2f)
                    //Input.GetKey(KeyCode.LeftControl))
                {
                    textManager.DrawAllText();
                    if (skipTimer > 0.5f / skipSpeed)
                    {
                        skipTimer = 0;
                        DrawNext();
                    }
                }
            }
        }
    }

    void DrawNext()
    {
        //文章の最後まで表示し終わっているか
        if (nowRead.Count - 1 <= DataManager.Instance.endLine && Fade.Instance.isFade != true)
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
                if (DataManager.Instance.nowReadCharcterID != -1)
                {
                    DataManager.Instance.nowReadCharcterID = -1;
                    DataManager.Instance.endLine = 0;
                    Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("Menu",true); });
                }
                else if (DataManager.Instance.nowReadStoryID == -1)
                {
                    //ADVシーンを終了
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    Debug.Log(DataManager.Instance.nowReadStoryID);
                    //読んでいたシーンがプロローグなら攻略キャラ選択へ移動
                    Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("CharacterChoice"); });

                    //SceneChanger.LoadScene("CharacterChoice");
                }
                else if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
                {
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    Debug.Log(DataManager.Instance.nowReadStoryID);
                    //パラメータがミニゲームだったらMiniGameシーンへ
                    Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MiniGame", true); });
                    //SceneChanger.LoadScene("MiniGame", true);
                }
                else
                {
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    Debug.Log(DataManager.Instance.nowReadStoryID);
                    //ADVシーンを終了
                    //ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
                    //インターミッションに移動
                    isIntermission = true;
                    textManager.IntermissionTextChange();
                    graphicManager.ChangeCanvasNext();
                    textManager.ChangeCanvasNext();
                }
            }
        }
        else
        {
            if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
            {
                DataManager.Instance.endLine++;
                //パラメータがミニゲームだったらMiniGameシーンへ
                Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MiniGame", true); });
                //SceneChanger.LoadScene("MiniGame", true);
            }
            else
            {
                graphicManager.Reset();
                textManager.ShiftNextText();
                graphicManager.DrawCharacter(nowRead);
                //graphicManager.DrawBack(nowRead[DataManager.Instance.endLine].backGroundID);
            }
        }
    }
}