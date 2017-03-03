using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goal : MonoBehaviour
{

    private int nowCharacterID = DataManager.Instance.masteringData.masteringCharacterID;
    [SerializeField]
    GameObject goalPopUp;
    [SerializeField]
    Image goalImage;

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
            ///////////////////////////////////////////////////////////////////
            if (SceneChanger.GetBeforeSceneName() == "MyPage")
            {
                if (DataManager.Instance.nowReadCharcterID == 0)
                {
                    SoundManager.Instance.PlayVoice("tatsumi_14");

                }

                if (DataManager.Instance.nowReadCharcterID == 1)
                {

                    SoundManager.Instance.PlayVoice("yusuke_12");
                }


            }

            else
            {
                if (nowCharacterID == 0)
                {
                    SoundManager.Instance.PlayVoice("tatsumi_14");
                }

                if (nowCharacterID == 1)
                {
                    SoundManager.Instance.PlayVoice("yusuke_12");
                }


            }
            ///////////////////////////////////////////////////////////////




            SoundManager.Instance.PlaySE("taiko goal");
            goalImage.enabled=true;
           // goalPopUp.SetActive(true);
            isGoal = true;
            
            StartCoroutine(ChangeScene());
        }
    }
}