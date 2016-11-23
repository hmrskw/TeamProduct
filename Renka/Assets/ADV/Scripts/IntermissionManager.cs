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
            DataManager.Instance.SaveMasteringData();
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
