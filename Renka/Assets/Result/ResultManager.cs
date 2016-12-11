using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    [SerializeField]
    Text resultText;

    [SerializeField]
    GameObject[] life = new GameObject[3];

	// Use this for initialization
	void Start () {
        for(int i = 0; i < DataManager.Instance.minigameHp; i++)
        {
            life[i].SetActive(true);
        }
        SetResultText();
        Fade.Instance.FadeOut(1f, null);

    }

    void Update()
    {
        if (InputManager.Instance.IsTouchBegan())
        {
            SceneChanger.LoadBeforeScene(true);
        }
    }

    void SetResultText()
    {
        string word = "成功";
        if(DataManager.Instance.minigameHp == 3)
        {
            word = "大成功";
        }
        else if(DataManager.Instance.minigameHp <= 0)
        {
            word = "失敗";
        }
        resultText.text = word;
    }
}
