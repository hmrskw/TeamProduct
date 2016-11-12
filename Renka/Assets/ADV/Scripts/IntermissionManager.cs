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
        SceneChanger.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        SceneChanger.LoadScene("MyPage");
    }
}
