using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChalacterSelectButton(string str)
    {
        if (str == "tatsumi")
        {
            Debug.Log(str);
        }

        if (str == "yusuke")
        {
            Debug.Log(str);
        }
    }

    public void DifficultySelectButton(string str)
    {
        if (str == "easy")
        {
            Debug.Log(str);
        }

        if (str == "normal")
        {
            Debug.Log(str);
        }

        if (str == "hard")
        {
            Debug.Log(str);
        }
    }

    public void EnterButton()
    {
        Debug.Log("Enter");
    }

    public void BackButton()
    {
        Debug.Log("Back");
    }

}
