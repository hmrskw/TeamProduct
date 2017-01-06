using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoiceCheck : MonoBehaviour {
    public bool isPassingCheck = false;

    [SerializeField]
    Text text;

    private int point;
    public int Point { set { point = value; } get { return point;} }

    public string Text { set { text.text = value; } get {return text.text; } }

    public void Determination()
    {
        gameObject.SetActive(false);
        isPassingCheck = true;
    }

    public void BackToChoice()
    {
        gameObject.SetActive(false);
    }
}
