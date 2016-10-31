using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntermissionManager : MonoBehaviour
{

    public void OnNextClick()
    {
        Debug.Log("OnNextClick");
    }

    public void OnMyPageClick()
    {
        Debug.Log("OnMyPageClick");
        DataManager.Instance.endLine = 0;
        Debug.Log( DataManager.Instance.endLine );
        SceneManager.LoadScene("MyPage");
    }
}
