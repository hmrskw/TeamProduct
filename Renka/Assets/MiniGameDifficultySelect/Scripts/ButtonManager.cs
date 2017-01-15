using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {


    // Use this for initialization
    void Start()
    {
        Fade.Instance.FadeOut(0.5f, null);
        DataManager.Instance.nowReadCharcterID = 0;
        DataManager.Instance.difficulty = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChalacterSelectButton(string str)
    {
        if (str == "tatsumi")
        {
            DataManager.Instance.nowReadCharcterID = 0;
            Debug.Log(str);
        }

        if (str == "yusuke")
        {
            DataManager.Instance.nowReadCharcterID = 1;
            Debug.Log(str);
        }
    }

    public void DifficultySelectButton(string str)
    {
        if (str == "easy")
        {
            DataManager.Instance.difficulty = 0;
            Debug.Log(str);
        }

        if (str == "normal")
        {
            DataManager.Instance.difficulty = 1;
            Debug.Log(str);
        }

        if (str == "hard")
        {
            DataManager.Instance.difficulty = 2;
            Debug.Log(str);
        }
    }

    public void EnterButton()
    {
        Debug.Log(DataManager.Instance.difficulty);
        Debug.Log(DataManager.Instance.nowReadCharcterID);
        SceneChanger.LoadScene("MiniGame");
    }

    public void BackButton()
    {
        SceneChanger.LoadScene("MyPage");
        Debug.Log("Back");
    }

}
