using UnityEngine;
using System.Collections;

public class MousePlayerMove : MonoBehaviour {

    private Vector3 prevPos;
    private Vector3 prevPosY;
    private float prevFrameReminder;
    private float prevFrameReminderY;

    [SerializeField, Range(-4f, -1f)]
    float leftLimitPos;
    [SerializeField, Range(1f, 4f)]
    float rightLimitPos;
    [SerializeField]
    float moveSpeed;

    private Rigidbody rb;

    private bool canJump = true;



    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnTriggerEnter(Collider col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);

        
        if(layerName == "Ground")
        {
   
            canJump = true;
        }


    }

    void OnTriggerExit(Collider col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);

        if (layerName == "Ground")
        {
            canJump = false;
        }


    }



    // Update is called once per frame
    void Update () {

        
        if (prevFrameReminderY>=0.3)
        {
          // rb.AddForce(Vector3.up * 300);
        }
        
        //テスト
        if (Input.GetKeyDown(KeyCode.Space))
            {
            if(canJump==true)
            {
                rb.AddForce(Vector3.up * 250);

            }

            

        }

       
        //クリック押し込みの瞬間
        if (/*Input.GetMouseButtonDown(0)*/InputManager.Instance.IsTouchBegan())
        {
            float x = InputManager.Instance.GetTouchPosition().x;//Input.mousePosition.x;
            //xの値を画面サイズで割って0~1のサイズに
            prevPos = new Vector3(x / Screen.width, 0, 0);

            //ジャンプ処理
            float y = InputManager.Instance.GetTouchPosition().y;
            prevPosY = new Vector3(0, y / Screen.height, 0);


        }

        //クリックしている間
        if (/*Input.GetMouseButton(0)*/InputManager.Instance.IsTouchMoved())
        {

            //マウスのポジション取得
            float x = InputManager.Instance.GetTouchPosition().x;//Input.mousePosition.x;
            //前フレームとの移動量の差
            prevFrameReminder = x / Screen.width - prevPos.x;
            
            //現在の位置を保存しておく
            prevPos.x = x / Screen.width;
           // Debug.Log(prevFrameReminder);
            //Debug.Log("mae"+prevPos.x);



            if (transform.position.x <= leftLimitPos)
            {
                transform.position = new Vector3(leftLimitPos, -2, 0);
                //もし移動量がプラスの値の時だけ移動
                transform.Translate(Mathf.Max(prevFrameReminder * moveSpeed, 0), 0, 0);

            }

            else if (transform.position.x >= rightLimitPos)
            {
                transform.position = new Vector3(rightLimitPos, -2, 0);
                //もし移動量がマイナスの値の時だけ移動
                transform.Translate(Mathf.Min(prevFrameReminder * moveSpeed, 0), 0, 0);

            }

            //移動させる 
            else transform.Translate(prevFrameReminder*moveSpeed, 0, 0);


            //ジャンプ処理
            //マウスのポジション取得
            float y = InputManager.Instance.GetTouchPosition().y;
           
            
            //前フレームとの移動量の差
            prevFrameReminderY = (y / Screen.height) - prevPosY.y;
            //現在の位置を保存しておく
            prevPosY.y = y / Screen.height;
            
        }







    }


    




    }
