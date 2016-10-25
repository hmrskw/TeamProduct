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

    //定数
    public float roadScrollSpeed;

    [HideInInspector]
    public float ScrollSpeed;

	// Use this for initialization
	void Start () {
        ScrollSpeed = roadScrollSpeed;
        StageShuffle();
        StartCoroutine(test());
    }
    IEnumerator test()
    {

        yield return new WaitForSeconds(3.0f);
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
