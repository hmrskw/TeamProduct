using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fade : MonoBehaviour {
    [SerializeField]
    RawImage[] life;
    
    [SerializeField]
    Text resultText;
    [SerializeField]
    Image commentArea;
    [SerializeField]//arufa 100
    Image commentImage;
    [SerializeField]
    Text comment;
    [SerializeField]
    GameObject KihudaPopUp;

    [SerializeField]
    GameObject NextSceneButton;

    [SerializeField]
    float fadeSpeed;

    
    

    //Color color;
    int nowPhase = 0;
    // Use this for initialization
    void Start () {
        StartCoroutine(FadeIn());
       
        
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(LifeFade());
        yield return StartCoroutine(ResultTextFade());
        yield return StartCoroutine(CommentAreaFade());
        yield return StartCoroutine(CommentTextFade());

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(KihudaPopUpEnable());
        yield return StartCoroutine(ButtonFade());
    }

    IEnumerator LifeFade()
    {
        Debug.Log("LifeFade");
        Color color;

        color = life[0].color;
        color.a = 0;

        while (color.a <= 1)
        {
            color.a += fadeSpeed;
            for (int i = 0; i < life.Length; i++)
            {

                life[i].color = color;

            }
            yield return null;
        }
    }


    IEnumerator ResultTextFade()
    {
        Debug.Log("resultTextFade");
        Color color;

        color = resultText.color;
        color.a = 0;

        while (color.a <= 1)
        {
            color.a += fadeSpeed;
            
            resultText.color = color;

            
            yield return null;
        }
    }

    IEnumerator CommentAreaFade()
    {
        Debug.Log("CommentAreaFade");
        Color color;

        color = commentArea.color;
        color.a = 0;



        while (color.a <= 1)
        {
            color.a += fadeSpeed;

            commentArea.color = color;
            commentImage.color =new Color(255,255,255,color.a/2) ;


            yield return null;
        }
    }

    IEnumerator CommentTextFade()
    {
        Debug.Log("CommentAreaFade");
        Color color;

        color = comment.color;
        color.a = 0;



        while (color.a <= 1)
        {
            color.a += fadeSpeed;

            comment.color = color;
            


            yield return null;
        }
    }


    IEnumerator KihudaPopUpEnable()
    {
        if (SceneChanger.GetBeforeSceneName() == "ADV")
        {
            if (DataManager.Instance.minigameHp == 3)
            {
                KihudaPopUp.SetActive(true);

                yield return null;
            }
        }
    }

    IEnumerator ButtonFade()
    {
        NextSceneButton.SetActive(true);
       yield return null;
    }

    // Update is called once per frame
    void Update () {

            //if (color.a <= 1)
            //{
            //    color.a += 0.05f;

            //}
            //else
            //{
            //    nowPhase = 1;
                
            //}


            //for (int i = 0; i < life.Length; i++)
            //{

            //    life[i].color = color;

            //}
        
      






        }
}
