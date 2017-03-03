using UnityEngine;
using System.Collections;

public class ActiveFires : MonoBehaviour
{
    [SerializeField]
    ChouchinMove chouchin;
    [SerializeField]
    GameObject[] fires;
    int moveCount = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (chouchin.isRight == false)
        {
            

            if (chouchin.totalMove >= moveCount)
            {
                
                fires[7 - moveCount].SetActive(true);
                if (moveCount < 7)
                {
                    moveCount += 1;
                }

            }

        }


        //Debug.Log(chouchin.totalMove);
        if (chouchin.isRight == true)
        {


            if (chouchin.totalMove >= moveCount)
            {
               // Debug.Log(moveCount);
                fires[moveCount - 1].SetActive(true);
                if (moveCount < 7)
                {
                    moveCount += 1;
                }

            }

         

        }
    }

}