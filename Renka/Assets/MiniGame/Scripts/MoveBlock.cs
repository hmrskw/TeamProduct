using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour
{
    public bool isInRange = false;

    [Tooltip("障害物が右から左ならTrue、左から右ならfalse")]
    [SerializeField]
    private bool isRight = true;

    [SerializeField]
    private float leftLimitPos;

    [SerializeField]
    private float rightLimitPos;

    private float nowTime;
    private int nowTextureNum;

    [SerializeField]
    [Tooltip("キャラクターのテクスチャが切り替わるまでの時間")]
    float animationChangeTime;

    [SerializeField]
    private Texture[] nekoAnimationImages;
    private Material nekoMate;

    public float speed=0.1f;

   
    // Use this for initialization
    void Start()
    {
        nekoMate = GetComponent<Renderer>().material;

        if(isRight==true)
        {
            nekoMate.mainTextureScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        if (isInRange == true)
        {

            
            if (isRight == true)
            {
                if (transform.position.x >= leftLimitPos)
                {
                    TextureAnim();
                    transform.Translate(-speed, 0, 0);

                }
            }

            else
            {
                if (transform.position.x <= rightLimitPos)
                {
                    TextureAnim();
                    transform.Translate(speed, 0, 0);
                }

            }
        }
    }

    void TextureAnim()
    {

        
        nowTime += Time.deltaTime;

        //Debug.Log(nowTextureNum);

        if (nowTime > animationChangeTime)
        {

            nowTime = 0f;
            nowTextureNum++;


            if (nowTextureNum >= nekoAnimationImages.Length)
            {

                nowTextureNum = 0;
            }

        }
        nekoMate.mainTexture = nekoAnimationImages[nowTextureNum];
    }

}
