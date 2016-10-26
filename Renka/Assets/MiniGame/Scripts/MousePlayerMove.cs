using UnityEngine;
using System.Collections;

public class MousePlayerMove : MonoBehaviour {

    private Vector3 prevPos;
    private float moveSpeed;

    [SerializeField, Range(-4f, -1f)]
    float leftLimitPos;
    [SerializeField, Range(1f, 4f)]
    float rightLimitPos;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //クリック押し込みの瞬間
        if (Input.GetMouseButtonDown(0))
        {
            float x = Input.mousePosition.x;
            //xの値を画面サイズで割って0~1のサイズに
            prevPos = new Vector3(x / Screen.width, 0, 0);
        }

        //クリックしている間
        if (Input.GetMouseButton(0))
        {
            //マウスのポジション取得
            float x = Input.mousePosition.x;
            //前フレームとの移動量の差
            moveSpeed = x / Screen.width - prevPos.x;
            //現在の位置を保存しておく
            prevPos.x = x / Screen.width;

            
            if (transform.position.x <= leftLimitPos)
            {
                transform.position = new Vector3(leftLimitPos,-2f, -0.3f);

            }

            if (transform.position.x >= rightLimitPos)
            {
                transform.position = new Vector3(rightLimitPos, -2, -0.3f);

            }

            //移動させる 
            transform.Translate(moveSpeed*4f, 0, 0);
        }
        



      


    }


    




    }
