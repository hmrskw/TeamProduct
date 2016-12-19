using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : MonoBehaviour
{

    [SerializeField]
    GameObject[] roads;
    [SerializeField]
    GameObject[] easyRoads;
    [SerializeField]
    GameObject[] normalRoads;
    [SerializeField]
    GameObject[] hardRoads;


    [SerializeField]
    GameObject[] startGoal = new GameObject[2];

    [SerializeField]
    Goal goal;
    [SerializeField]
    Text startCountText;

    [SerializeField]
    GameObject waitClickText;



    [HideInInspector]
    public float gagePlayerPos;
    private float scrollLength;

    //定数
    public float roadScrollSpeed;

    [HideInInspector]
    public float ScrollSpeed;
    [SerializeField]
    [Tooltip("道(パターン)１つの長さ")]
    float roadLength;

    // Use this for initialization
    void Start()
    {
        Fade.Instance.FadeOut(0.5f, null);

        ScrollSpeed = roadScrollSpeed;
        StartCoroutine(stageMoveCor());

        //難易度EASY
        if(DataManager.Instance.difficulty==0)
        {
            Debug.Log("easy");
            roads = easyRoads;
        }

        //難易度NORMAL
        if (DataManager.Instance.difficulty == 1)
        {
            Debug.Log("normal");
            roads = normalRoads;
        }

        //難易度HARD
        if (DataManager.Instance.difficulty == 2)
        {
            Debug.Log("hard");
            roads = hardRoads;
        }
        StageShuffle();
    }

    //スタート前のカウント    
    IEnumerator StartCount()
    {

        startCountText.text = "参";
        yield return new WaitForSeconds(1f);

        startCountText.text = "弐";
        yield return new WaitForSeconds(1f);

        startCountText.text = "壱";
        yield return new WaitForSeconds(1f);

        startCountText.text = "始め";
        yield return new WaitForSeconds(1f);
        startCountText.text = "";
        yield return null;

    }

    IEnumerator waitClick()
    {
        while(true)
        {
            if (Fade.Instance.isFade == false)
            {

                if (InputManager.Instance.IsTouchBegan())
                {
                    waitClickText.SetActive(false);
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator stageMoveCor()
    {
        ScrollSpeed = 0.0f;
        
        //クリックされてから
        yield return StartCoroutine(waitClick());

        //開始前に4秒待ってカウントダウン表示
        yield return StartCoroutine(StartCount());

        ScrollSpeed = roadScrollSpeed;
        //ゴールするまで
        while (!goal.isGoal)
        {
            StageMove();
            yield return null;

        }

        yield return null;
    }




    // Update is called once per frame
    void Update()
    {

        //StageMove();


    }

    void StageMove()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].transform.Translate(0, 0, ScrollSpeed);
        }
        for (int i = 0; i < startGoal.Length; i++)
        {
            startGoal[i].transform.Translate(0, 0, ScrollSpeed);
        }

        //ステージがスクロールした長さを取得(進行度ゲージに使う)
        scrollLength -= ScrollSpeed;
        //ゲージの進行度合いを0~1の値に
        gagePlayerPos = scrollLength / (roads.Length * roadLength);
        //Debug.Log(scrollLength / (roads.Length * roadLength));

    }

    void StageShuffle()
    {
        roads = roads.OrderBy(i => Guid.NewGuid()).ToArray();
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].transform.position = new Vector3(0, 0, i * 30 + 5);
        }
    }
}
