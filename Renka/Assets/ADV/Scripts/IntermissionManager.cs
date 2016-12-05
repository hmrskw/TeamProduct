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
        Fade.Instance.FadeOut(1f, null);

        if (DataManager.Instance.isEndStory())
        {
            button.interactable = false;
        }
    }

    void OnEnable()
    {
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
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("ADV"); });
        }
        //SceneChanger.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        if (Fade.Instance.isFade == false)
        {
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MyPage"); });
        }

        //SceneChanger.LoadScene("MyPage");
    }
}
