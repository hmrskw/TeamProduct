using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ADVManager : MonoBehaviour
{

    ChoiceManager choiceManager;

    TextManager textManager;

    GraphicManager graphicManager;

    [SerializeField]
    IntermissionManager intermissionManager;

    string key;

    List<ConvertADVData.ADVData> nowRead;

    [SerializeField,Range(1,5)]
    int skipSpeed;

    [SerializeField]
    PopManager popup;

    [SerializeField]
    GameObject configPopup;

    [SerializeField]
    GameObject MenuButton;

    float skipTimer = 0;

    bool isIntermission = false;


    void Start()
    {
        nowRead = ConvertADVData.Instance.advData;
        choiceManager = GetComponent<ChoiceManager>();
        textManager = GetComponent<TextManager>();
        graphicManager = GetComponent<GraphicManager>();

        intermissionManager.BeforeLikeabillity = DataManager.Instance.masteringData.likeabillity;

        if (SceneChanger.GetBeforeSceneName(true) == "Result")
        {
            //SceneChanger.GetBeforeSceneName(true);
            //DataManager.Instance.endLine = 0;
            //DataManager.Instance.nowReadChapterID++;
            //ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
            //インターミッションに移動
            isIntermission = true;
            textManager.IntermissionTextChange();
            graphicManager.ChangeCanvasNext();
            textManager.ChangeCanvasNext();
            MenuButton.SetActive(false);
        }
        else if (SceneChanger.GetBeforeSceneName() != "Menu")
        {
            //SceneChanger.GetBeforeSceneName(true);
            ConvertADVData.Instance.SetMasteringCharacterLastStoryID();
        }
    }

    void Update()
    {
        if (isIntermission == true) return;
        if (Fade.Instance.isFade == true) return;
        if (popup.isDrawpopup == true) return;
        //if (configPopup.activeInHierarchy == true) return;

        if (choiceManager.isActiveChoices)
        {
            //選択肢表示中ならキーを取得
            key = choiceManager.Choice();
            if (key != null)
            {
                //取得したキーから読むADVデータを切り替える
                nowRead = ConvertADVData.Instance.choiceADVData[key];
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
        /////////////////////////////////////////////////////////////////////////////////////////////////
        else if(nowRead[DataManager.Instance.endLine].command == "bgmplay" || nowRead[DataManager.Instance.endLine].command == "bgmPlay")
        {
            SoundManager.Instance.PlayBGM(nowRead[DataManager.Instance.endLine].parameter);
            DrawNext();
        }
        else if (nowRead[DataManager.Instance.endLine].command == "bgmstop" || nowRead[DataManager.Instance.endLine].command == "bgmStop")
        {
            SoundManager.Instance.StopBGM();
            DrawNext();
        }
        else if (nowRead[DataManager.Instance.endLine].command == "seplay" || nowRead[DataManager.Instance.endLine].command == "sePlay")
        {
            SoundManager.Instance.PlaySE(nowRead[DataManager.Instance.endLine].parameter);
            DrawNext();
        }
        else if (nowRead[DataManager.Instance.endLine].command == "sestop" || nowRead[DataManager.Instance.endLine].command == "seStop")
        {
            SoundManager.Instance.StopSE();
            DrawNext();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////
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
                    textManager.logArray.Add(
                        new TextManager.LogData(
                            nowRead[DataManager.Instance.endLine].sendCharacter,
                            nowRead[DataManager.Instance.endLine].text)
                        );
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
                    textManager.logArray.Add(new TextManager.LogData(
                            nowRead[DataManager.Instance.endLine].sendCharacter,
                            nowRead[DataManager.Instance.endLine].text)
                        );
                    DrawNext();
                }
            }
            //既読スキップ処理
            if (DataManager.Instance.nowReadStoryID < DataManager.Instance.masteringData.masteringCharacterLastStoryID)
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
                nowRead = ConvertADVData.Instance.advData;
                textManager.nowRead = nowRead;
                textManager.ShiftNextText();
            }
            else
            {
                if (SceneChanger.GetBeforeSceneName(true) == "Menu")
                {
                    //SaveData.LoadMasteringData();
                    DataManager.Instance.nowReadCharcterID = -1;
                    DataManager.Instance.endLine = 0;
                    Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("Menu",true); });
                }
                else if (DataManager.Instance.nowReadStoryID == -1)
                {
                    //ADVシーンを終了
                    DataManager.Instance.endLine = 0;
                    DataManager.Instance.nowReadStoryID++;
                    DataManager.Instance.nowReadChapterID++;
                    textManager.logArray.Clear();
                    Debug.Log(DataManager.Instance.nowReadStoryID);
                    //読んでいたシーンがプロローグなら攻略キャラ選択へ移動
                    Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("CharacterChoice"); });

                    //SceneChanger.LoadScene("CharacterChoice");
                }
                else if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
                {
                    //DataManager.Instance.nowReadStoryID++;
                    //if (!DataManager.Instance.isEndStory())
                    //{
                    //    DataManager.Instance.nowReadStoryID++;
                    //    //DataManager.Instance.nowReadStoryID = 0;
                    //    DataManager.Instance.nowReadChapterID++;
                    //}
                    //パラメータがミニゲームだったらMiniGameシーンへ
                    if (DataManager.Instance.masteringData.readChapterID == 2) {
                        DataManager.Instance.difficulty = 0;
                    }
                    else if(DataManager.Instance.masteringData.readChapterID == 6)
                    {
                        DataManager.Instance.difficulty = 1;
                    }
                    else if (DataManager.Instance.masteringData.readChapterID == 8)
                    {
                        DataManager.Instance.difficulty = 2;
                    }
                    else
                    {
                        DataManager.Instance.difficulty = 0;
                    }

                    textManager.logArray.Clear();
                    Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MiniGame", true); });
                    //SceneChanger.LoadScene("MiniGame", true);
                }
                else
                {
                    //DataManager.Instance.endLine = 0;
                    //DataManager.Instance.nowReadStoryID++;
                    //ADVシーンを終了
                    //ConvertADVdata.Instance.SetMasteringCharacterLastStoryID();
                    //インターミッションに移動
                    isIntermission = true;
                    textManager.logArray.Clear();
                    textManager.IntermissionTextChange();
                    graphicManager.ChangeCanvasNext();
                    textManager.ChangeCanvasNext();
                    MenuButton.SetActive(false);
                }
            }
        }
        else
        {
            if (nowRead[DataManager.Instance.endLine].parameter == "minigame")
            {
                DataManager.Instance.endLine++;
                //パラメータがミニゲームだったらMiniGameシーンへ
                Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MiniGame", true); });
                //SceneChanger.LoadScene("MiniGame", true);
            }
            else
            {
                graphicManager.Reset();
                textManager.ShiftNextText();
                graphicManager.DrawCharacter(nowRead);
                if (nowRead[DataManager.Instance.endLine].command == "voice")
                {
                    SoundManager.Instance.PlayVoice(nowRead[DataManager.Instance.endLine].parameter);
                }
                //graphicManager.DrawBack(nowRead[DataManager.Instance.endLine].backGroundID);
            }
        }
    }

    public void OpenMenu()
    {
        popup.isDrawpopup = true;
    }
}