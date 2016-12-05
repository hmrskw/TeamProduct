using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private StageManager stage = null;

    [HideInInspector]
    public int hp;
    [SerializeField]
    private GameObject[] hpImage;

    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private Sprite[] faceIconImages;

    //テクスチャアニメーション//////////////////////////////
    [SerializeField]
    private Texture[] characterAnimationImages;

    [SerializeField][Tooltip("キャラクターのテクスチャが切り替わるまでの時間")]
    float animationChangeTime;

    private float nowTime;
    private int nowTextureNum;
    // ///////////////////////////////////////////////////

    [SerializeField]
    private GameObject faceIconUi;

    private Image face;
    private Material playerMate;

    private bool is_hit = false;

    void Start()
    {

        hp = hpImage.Length;
        face = faceIconUi.GetComponent<Image>();
        playerMate = GetComponent<Renderer>().material;
        

    }

   void Update()
    {


        
        TextureAnim();
        if (hp>0)
        {
            face.sprite = faceIconImages[hp];

        }
    }

    //キャラクターの走るアニメーション
    void TextureAnim()
    {
        if(stage.ScrollSpeed < -0.1f)
        {

            nowTime += Time.deltaTime;
        }
        
        if (nowTime>animationChangeTime)
        {
            
            nowTime = 0f;
            nowTextureNum++;
            if(nowTextureNum>=characterAnimationImages.Length)
            {
                
                nowTextureNum = 0;
            }
        }
        playerMate.mainTexture = characterAnimationImages[nowTextureNum];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Goal")
        {
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        //ステージの移動を止める
        stage.ScrollSpeed = 0.0f;


        hp -= 1;
        if (hp>0)
        {
            //HPの表示を1つ減らす
            hpImage[hp].SetActive(false);
        }
        else
        {
            face.sprite = faceIconImages[hp];
            hpImage[0].SetActive(false);
            //ステージスクロールをとめる
            stage.ScrollSpeed = 0.0f;
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(1);
            SceneChanger.LoadBeforeScene(true);
        }

        


        yield return StartCoroutine(DamageEffect());
        
        //ステージの移動を元に戻す
        stage.ScrollSpeed = stage.roadScrollSpeed;
    }


    //作成中
    IEnumerator caracterAnimation()
    {
        while (stage.ScrollSpeed<-0.1f)
        {
            yield return new WaitForSeconds(1);
            
            playerMate.mainTexture = characterAnimationImages[1];
            yield return new WaitForSeconds(1);
             playerMate.mainTexture = characterAnimationImages[0];
           
            yield return null;
        }

            

    }

    [SerializeField]
    bool _alphaRound = false;

    IEnumerator DamageEffect()
    {
        var renderer = GetComponent<Renderer>();

        float interval = 0f;
        while (interval < 1f)
        {
            var angle = interval * Mathf.PI * 4f;
            var alpha = (Mathf.Sin(angle) * 0.5f) + 0.5f;
            if (_alphaRound) { alpha = Mathf.RoundToInt(alpha); }
            
            interval += Time.deltaTime;

            
            var color = Color.white;
            color.a = alpha;
            renderer.material.color = color;

            yield return null;
          
        }

        renderer.material.color = Color.white;
    }
}
