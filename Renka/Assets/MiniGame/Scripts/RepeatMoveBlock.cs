using UnityEngine;
using System.Collections;

public class RepeatMoveBlock : MonoBehaviour {
    [SerializeField]
    float leftLimitPos;

    [SerializeField]
    float rightLimitPos;
    [SerializeField]
    bool goRight=true;
    [SerializeField]
    float speed;

    [SerializeField]
    [Tooltip("キャラクターのテクスチャが切り替わるまでの時間")]
    float animationChangeTime;

    [SerializeField]
    private Texture[] nekoAnimationImages;
    private Material nekoMate;

    private float nowTime;
    private int nowTextureNum;

    // Use this for initialization
    void Start () {

        nekoMate = GetComponent<Renderer>().material;

        if (goRight == true)
        {
            nekoMate.mainTextureScale = new Vector2(1, 1);
        }

    }
	
	// Update is called once per frame
	void Update () {
	if(goRight==false)
        {
            if(transform.position.x>=leftLimitPos)
            {
                transform.Translate(-speed, 0, 0);
                TextureAnim();
                nekoMate.mainTextureScale = new Vector2(-1, 1);

            }
            else
            {
                goRight = true;
            }
        }

    else
        {
            if (transform.position.x <= rightLimitPos)
            {
                transform.Translate(speed, 0, 0);
                TextureAnim();
                nekoMate.mainTextureScale = new Vector2(1, 1);

            }
            else
            {
                goRight = false;
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
