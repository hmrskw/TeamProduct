using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntermissionManager : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    bool canSave;

    [SerializeField]
    Slider likeabillityGage;

    [SerializeField]
    Image[] items;

    public float BeforeLikeabillity { get; set; }

    void Start()
    {
        Fade.Instance.FadeOut(0.5f, 
            () => {
                StartCoroutine(
                    SliderValueChanger(
                        likeabillityGage,
                        BeforeLikeabillity / 50f,
                        DataManager.Instance.masteringData.likeabillity / 50f
                    )
                );
            }
        );

        likeabillityGage.value = BeforeLikeabillity / 50f;
        DataManager.Instance.endLine = 0;

        for (int i = 0;i<items.Length ; i++)
        {
            if(i < DataManager.Instance.masteringData.itemNum)
            {
                items[i].color = Color.white;
            }
            else
            {
                items[i].color = Color.black;
            }
        }

        if (DataManager.Instance.isEndChapter() && DataManager.Instance.isEndStory())
        {
            button.interactable = false;
        }
        if (DataManager.Instance.isEndStory())
        {
            if (DataManager.Instance.masteringData.masteringCharacterLastChapterID - 3 == DataManager.Instance.nowReadChapterID)
            {
                if (DataManager.Instance.baseline <= DataManager.Instance.masteringData.likeabillity)
                {
                    DataManager.Instance.nowReadChapterID++;
                }
                DataManager.Instance.nowReadChapterID++;
                DataManager.Instance.nowReadStoryID = 0;
            }
            else if(DataManager.Instance.isEndChapter() == false)
            {
                DataManager.Instance.nowReadChapterID++;
                DataManager.Instance.nowReadStoryID = 0;
            }
            else if(DataManager.Instance.masteringData.itemNum >= 3 && DataManager.Instance.nowReadChapterID == DataManager.Instance.masteringData.masteringCharacterLastChapterID - 1)
            {
                button.interactable = true;
                DataManager.Instance.nowReadChapterID = DataManager.Instance.masteringData.masteringCharacterLastChapterID;
                DataManager.Instance.nowReadStoryID = 0;
            }
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

    public IEnumerator SliderValueChanger(Slider slider, float startValue, float endValue)
    {
        float wipeTime = 1;
        float startTime = Time.timeSinceLevelLoad;
        float diff = 0f;

        while (diff < wipeTime)
        {
            diff = Time.timeSinceLevelLoad - startTime;

            float rate = diff / wipeTime;
            float a = (endValue - startValue) * rate;
            slider.value = startValue + a;

            yield return null;
        }
    }
}
