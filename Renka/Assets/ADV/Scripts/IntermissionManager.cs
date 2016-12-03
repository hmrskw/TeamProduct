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
        SceneChanger.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        SceneChanger.LoadScene("MyPage");
    }
}
