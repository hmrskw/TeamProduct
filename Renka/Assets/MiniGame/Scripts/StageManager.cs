using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : MonoBehaviour {

    [SerializeField]
    GameObject[] roads;
    [SerializeField]
    GameObject[] startGoal =new GameObject [2];

    [SerializeField]
    Goal goal;

    

    [HideInInspector]
    public float gagePlayerPos;
    private float scrollLength;

    //定数
    public float roadScrollSpeed;

    [HideInInspector]
    public float ScrollSpeed;
    [SerializeField] [Tooltip("道(パターン)１つの長さ")]
    float roadLength;

	// Use this for initialization
	void Start () {
        ScrollSpeed = roadScrollSpeed;
        StageShuffle();
        StartCoroutine(stageMoveCor());
    }
    IEnumerator stageMoveCor()
    {
        ScrollSpeed =0.0f;

        //開始前に3秒待つ
        yield return new WaitForSeconds(3.0f);

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
	void Update () {

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
        roads = roads.OrderBy(i=>Guid.NewGuid()).ToArray();
        for(int i=0;i<roads.Length;i++)
        {
            roads[i].transform.position = new Vector3(0, 0, i * 30 + 5);
        }
    }
}
