using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResultManager : MonoBehaviour {
    //[SerializeField]
    //Text resultText;

    [SerializeField]
    Image resultImage;

    [SerializeField]
    Text commentText;

    [SerializeField]
    Sprite[] resultSprits;


    [System.Serializable]
    public struct characterComments
    {
        //表情画像の配列
        public Comments[] comments;
    }

    [System.Serializable]
    public struct Comments
    {
        public string comment;
        public string voiceName;
    }

    [SerializeField, Tooltip("キャラクターのコメント")]
    characterComments[] comments;

    [SerializeField]
    GameObject[] life = new GameObject[3];

    [SerializeField]
    GameObject kihudaPopUp;

	// Use this for initialization
	void Start () {
        int minigameHp = DataManager.Instance.minigameHp;

        for (int i = 0; i < minigameHp; i++)
        {
            life[i].SetActive(true);
        }
        SetResultText(minigameHp);
        SetResultComment(minigameHp);
        Fade.Instance.FadeOut(0.5f, null);

    }

    void SetResultComment(int minigameHp_)
    {
        int charID = 0;

        if (SceneChanger.GetBeforeSceneName() == "MyPage")
        {
            charID = DataManager.Instance.nowReadCharcterID;
        }
        //前のシーンがマイページ以外なら
        else
        {
            charID = DataManager.Instance.masteringData.masteringCharacterID;
        }
        string word = "";

        if (minigameHp_ == 3)
        {
            word = comments[charID].comments[0].comment;
            DataManager.Instance.masteringData.itemNum++;
            SoundManager.Instance.PlayVoice(comments[charID].comments[0].voiceName);
        }
        else if (minigameHp_ <= 0)
        {
            word = comments[charID].comments[2].comment;
            SoundManager.Instance.PlayVoice(comments[charID].comments[2].voiceName);
        }
        else
        {
            word = comments[charID].comments[1].comment;
            SoundManager.Instance.PlayVoice(comments[charID].comments[1].voiceName);
        }

        commentText.text = word;
    }

    void Update()
    {
        //if (InputManager.Instance.IsTouchBegan())
        //{
        //    SceneChanger.LoadBeforeScene(true);
        //}

       
    }

    public void ButtonSceneChange()
    {
        SoundManager.Instance.StopVoice(false);
        SoundManager.Instance.PlaySE("botan");
        SceneChanger.LoadBeforeScene(true);
    }

    public void ReTryButton()
    {
        SoundManager.Instance.StopVoice(false);
        SoundManager.Instance.PlaySE("botan");
        SceneChanger.LoadScene("Minigame");
    }

    void SetResultText(int minigameHp_)
    {
        /*
        string word = "成功";
        if(minigameHp_ == 3)
        {
            word = "大成功";
        }
        else if(minigameHp_ <= 0)
        {
            word = "失敗";
        }
        resultText.text = word;
        */
        int id = 1;
        if (minigameHp_ == 3)
        {
            id = 0;
        }
        else if (minigameHp_ <= 0)
        {
            id = 2;
        }
        resultImage.sprite = resultSprits[id];

    }
}
