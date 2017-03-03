using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject goalPopUp;

    [SerializeField]
    StageManager stageManager;

    [SerializeField]
    Makimono resultMakimono;

    public bool isGoal = false;
    private bool isTouch = false;



    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        

        while(InputManager.Instance.IsTouchBegan()==false)
        {
            yield return null;
        }

        yield return StartCoroutine(resultMakimono.MakimonoScroll());

       
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.StopSE();
        //SceneChanger.LoadScene("MyPage");
        SceneChanger.LoadScene("Result");
        //SceneChanger.LoadBeforeScene(true);
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            stageManager.ScrollSpeed = 0.0f;
            SoundManager.Instance.PlaySE("taiko goal");
            goalPopUp.SetActive(true);
            isGoal = true;
            
            StartCoroutine(ChangeScene());
        }
    }
}