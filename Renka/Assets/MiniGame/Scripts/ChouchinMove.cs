using UnityEngine;
using System.Collections;

public class ChouchinMove : MonoBehaviour {

    public bool isInRange = false;

    [Tooltip("障害物が右から左ならTrue、左から右ならfalse")]
    [SerializeField]
    public bool isRight = true;

    [SerializeField]
    private float leftLimitPos;

    [SerializeField]
    private float rightLimitPos;

    [SerializeField]
    private float GroundPos;

    private int nowPhase = 0;
    [SerializeField]
    ChouchinRotate chouchinRotate;
    [HideInInspector]
    public float totalMove=0f;

    [SerializeField]
    GameObject chouchinLight;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isInRange==true)
        {
            //地面まで落ちる
            if (nowPhase == 0)
            {
                chouchinLight.SetActive(false);
                if (transform.position.y >= GroundPos)
                {
                    transform.Translate(0, -0.15f, 0);
                    

                }
                else
                {
                    nowPhase = 1;
                }
            }
            //転がる
            if(nowPhase==1)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                if (isRight==true)
                {
                    if (transform.position.x >= leftLimitPos)
                    {
                        //transform.eulerAngles +=new Vector3(0.0f, 0.0f, 10.0f);
                        //GetComponent<Rigidbody>().velocity = (transform.right * -2);
                        transform.Translate(-0.1f, 0, 0);
                        totalMove += 0.1f;
                        
                    }
                    else
                    {
                        chouchinRotate.enabled = false;
                    }
                }

                else
                {
                    if (transform.position.x <= rightLimitPos)
                    {
                       
                        transform.Translate(0.1f, 0, 0);
                        totalMove += 0.1f;

                    }
                    else
                    {
                        chouchinRotate.enabled = false;
                    }
                }
            }
        }
      

	
	}
}
