using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BacklogView : MonoBehaviour {

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text text;

    public void ViewBacklogText(string name_,string text_)
    {
        nameText.text = name_;
        text.text = text_;
    }
}
