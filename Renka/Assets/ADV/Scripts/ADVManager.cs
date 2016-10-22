using UnityEngine;
using System.Collections;

public class ADVManager : MonoBehaviour {

    ChoiceManager choiceManager;

    TextManager textManager;

    GraphicManager graphicManager;
    
    void Start () {
        choiceManager = GetComponent<ChoiceManager>();
        textManager = GetComponent<TextManager>();
        graphicManager = GetComponent<GraphicManager>();
    }
	
	void Update () {
        if (choiceManager.isActiveChoices)
        {
            choiceManager.Choice();
            if (choiceManager.isActiveChoices == false)
            {
                textManager.ShiftNextText();
            }
        }
        else
        {
            if (textManager.IsDrawAllText())
            {
                choiceManager.DrawChoice(DataManager.Instance.endLine);
            }
            if (Input.GetMouseButtonDown(0))
            {
                //文字送り表示中かどうか
                //文字送りが終わってるとき
                if (textManager.IsDrawAllText())
                {
                    graphicManager.Reset();
                    textManager.ShiftNextText();
                    graphicManager.DrawCharacter();
                }
                else
                {
                    textManager.DrawAllText();
                }
            }
        }


    }
}
