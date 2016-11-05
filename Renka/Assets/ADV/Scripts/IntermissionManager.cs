using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntermissionManager : MonoBehaviour
{
    [SerializeField]
    Button button;

    void Start()
    {
        if (DataManager.Instance.isEndStory())
        {
            button.interactable = false;
        }
    }
    public void OnNextClick()
    {
        DataManager.Instance.endLine = 0;
        SceneChanger.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        DataManager.Instance.endLine = 0;
        SceneChanger.LoadScene("MyPage");
    }
}
