using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
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
        Debug.Log(Input.mousePosition+"\nIM"+InputManager.Instance.GetTouchPosition());


        if (choiceManager.isActiveChoices)
        {
            key = choiceManager.Choice();
            if (/*choiceManager.isActiveChoices == false*/key != null)
            {
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
                    if (/*ConvertADVdata.Instance.advData*/nowRead.Count - 1 == DataManager.Instance.endLine)
                    {
                        if (DataManager.Instance.isChoiceText)
                        {
                            DataManager.Instance.isChoiceText = false;
                            DataManager.Instance.endLine = Convert.ToInt16(nowRead[DataManager.Instance.endLine].parameter);
                            nowRead = ConvertADVdata.Instance.advData;
                            textManager.nowRead = nowRead;
                            textManager.ShiftNextText();
                        }
                        else
                        {
                            //Debug.Log("init");
                            DataManager.Instance.endLine = 0;
                            SceneManager.LoadScene("MyPage");
                        }
                    }
                    else
                    {
                        graphicManager.Reset();
                        textManager.ShiftNextText();
                        graphicManager.DrawCharacter(nowRead);
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
