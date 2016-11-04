using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntermissionManager : MonoBehaviour
{

    public void OnNextClick()
    {
        DataManager.Instance.endLine = 0;
        SceneManager.LoadScene("ADV");
    }

    public void OnMyPageClick()
    {
        DataManager.Instance.endLine = 0;
        SceneManager.LoadScene("MyPage");
    }
}
