using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    [SerializeField]
    GameObject goalPopUp;

    public bool isGoal=false;

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MyPage");
        yield return null;
    }
    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player")
        {
            goalPopUp.SetActive(true);
            isGoal = true;
            StartCoroutine(ChangeScene());
        }
    }
}
