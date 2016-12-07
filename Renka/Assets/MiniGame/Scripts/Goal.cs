using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject goalPopUp;

    [SerializeField]
    StageManager stageManager;

    public bool isGoal = false;

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        //SceneChanger.LoadScene("MyPage");
        SceneChanger.LoadBeforeScene(true);
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            stageManager.ScrollSpeed = 0.0f;

            goalPopUp.SetActive(true);
            isGoal = true;
            StartCoroutine(ChangeScene());
        }
    }
}