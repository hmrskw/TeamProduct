using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntermissionManager : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    bool canSave;

    void Start()
    {
        Fade.Instance.FadeOut(0.5f, null);
        DataManager.Instance.endLine = 0;
        if (DataManager.Instance.isEndChapter() && DataManager.Instance.isEndStory())
        {
            button.interactable = false;
        }
        if (DataManager.Instance.isEndStory())
        {
            if (DataManager.Instance.masteringData.masteringCharacterLastChapterID - 2 == DataManager.Instance.nowReadChapterID)
            {
                if (DataManager.Instance.baseline <= DataManager.Instance.masteringData.likeabillity)
                {
                    DataManager.Instance.nowReadChapterID++;
                }
            }
            DataManager.Instance.nowReadChapterID++;
            DataManager.Instance.nowReadStoryID = 0;
        }
        else
        {
            DataManager.Instance.nowReadStoryID++;
        }
        if (canSave)
        {
            SaveData.SaveMasteringData();
            SaveData.SaveFinishedStoryData(DataManager.Instance.masteringData.masteringCharacterID);
        }
    }

    public void OnNextClick()
    {
        if (Fade.Instance.isFade == false)
        {
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("ADV"); });
        }
        //SceneChanger.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        if (Fade.Instance.isFade == false)
        {
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
        }

        //SceneChanger.LoadScene("MyPage");
    }
}
