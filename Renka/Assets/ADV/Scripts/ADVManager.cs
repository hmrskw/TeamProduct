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

    void Start()
    {
        nowRead = ConvertADVdata.Instance.advData;
        choiceManager = GetComponent<ChoiceManager>();
        textManager = GetComponent<TextManager>();
        graphicManager = GetComponent<GraphicManager>();
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
                    //文章の最後まで表示し終わっているか
                    if (/*ConvertADVdata.Instance.advData*/nowRead.Count - 1 <= DataManager.Instance.endLine)
                    {
                        if (DataManager.Instance.isChoiceText){
                            //選択肢で分岐した後の文章なら共通文章に戻る
                            DataManager.Instance.isChoiceText = false;
                            DataManager.Instance.endLine = Convert.ToInt16(nowRead[DataManager.Instance.endLine].parameter);
                            nowRead = ConvertADVdata.Instance.advData;
                            textManager.nowRead = nowRead;
                            textManager.ShiftNextText();
                        }
                        else {
                            //ADVシーンを終了
                            DataManager.Instance.endLine = 0;
                            DataManager.Instance.nowReadStoryID++;
                            if (DataManager.Instance.nowReadStoryID == 0)
                            {
                                //読んでいたシーンがプロローグなら攻略キャラ選択へ移動
                                SceneChanger.LoadScene("CharacterChoice");
                            }
                            else
                            {
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
                        }
                    }
                }
                else
                {
                    textManager.DrawAllText();
                }
            }
        }
    }
}
