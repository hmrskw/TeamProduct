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
    private Sprite[] tatsumiFaceIconImages;

    [SerializeField]
    private Sprite[] yusukeFaceIconImages;

    //テクスチャアニメーション//////////////////////////////
    [SerializeField]
    private Texture[] tatsumiAnimationImages;

    [SerializeField]
    private Texture[] yusukeAnimationImages;



    [SerializeField]
    [Tooltip("キャラクターのテクスチャが切り替わるまでの時間")]
    float animationChangeTime;

    private float nowTime;
    private int nowTextureNum;
    // ///////////////////////////////////////////////////

    [SerializeField]
    private GameObject faceIconUi;

    private Image face;
    private Material playerMate;

    private bool is_hit = false;

    //誰を攻略中か
    // 0. 辰巳
    // 1. 酉助
    // 2. 卯太郎
    // 3. 一午
    private int nowCharacterID = DataManager.Instance.masteringData.masteringCharacterID;

    void Start()
    {

        nowCharacterID = DataManager.Instance.masteringData.masteringCharacterID;

        hp = hpImage.Length;
        DataManager.Instance.minigameHp = hp;

        face = faceIconUi.GetComponent<Image>();
        playerMate = GetComponent<Renderer>().material;

        //前のシーンがマイページなら
        if (SceneChanger.GetBeforeSceneName() == "MyPage")
        {
            Debug.Log("mypage");

            //タツミ
            if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 0)
            {
                playerMate.mainTexture = tatsumiAnimationImages[0];
            }
            //ユウスケ
            if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 1)
            {

                playerMate.mainTexture = yusukeAnimationImages[0];

            }

        }

        //前のシーンがマイページ以外なら
        else
        {
            //タツミ
            if (nowCharacterID == 0)
            {
                playerMate.mainTexture = tatsumiAnimationImages[0];
            }
            //ユウスケ
            if (nowCharacterID == 1)
            {

                playerMate.mainTexture = yusukeAnimationImages[0];

            }

        }




    }

    void Update()
    {



        TextureAnim();
        if (hp > 0)
        {
            if (SceneChanger.GetBeforeSceneName() == "MyPage")
            {
                if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 0)
                {
                    face.sprite = tatsumiFaceIconImages[hp];
                }

                if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 1)
                {
                    face.sprite = yusukeFaceIconImages[hp];
                }
            }

            else
            {
                //タツミ
                if (nowCharacterID == 0)
                {
                    face.sprite = tatsumiFaceIconImages[hp];
                }
                //ユウスケ
                if (nowCharacterID == 1)
                {

                    face.sprite = yusukeFaceIconImages[hp];

                }

            }


        }
    }

    //キャラクターの走るアニメーション
    void TextureAnim()
    {
        if (stage.ScrollSpeed < -0.1f)
        {

            nowTime += Time.deltaTime;
        }

        if (nowTime > animationChangeTime)
        {

            nowTime = 0f;
            nowTextureNum++;


            if (SceneChanger.GetBeforeSceneName() == "MyPage")
            {
                //タツミ
                if (DataManager.Instance.nowReadCharcterID == 0)
                {
                    if (nowTextureNum >= tatsumiAnimationImages.Length)
                    {

                        nowTextureNum = 0;
                    }

                }

                //ユウスケ
                if (DataManager.Instance.nowReadCharcterID == 1)
                {
                    if (nowTextureNum >= yusukeAnimationImages.Length)
                    {

                        nowTextureNum = 0;
                    }

                }

            }
            //マイページシーン以外なら/////////////////////////////////
            else
            {
                //タツミ
                if (nowCharacterID == 0)
                {
                    if (nowTextureNum >= tatsumiAnimationImages.Length)
                    {

                        nowTextureNum = 0;
                    }

                }

                //ユウスケ
                if (nowCharacterID == 1)
                {
                    if (nowTextureNum >= yusukeAnimationImages.Length)
                    {

                        nowTextureNum = 0;
                    }

                }

            }




        }

        if (SceneChanger.GetBeforeSceneName() == "MyPage")
        {
            if (DataManager.Instance.nowReadCharcterID == 0)
            {
                playerMate.mainTexture = tatsumiAnimationImages[nowTextureNum];
            }

            if (DataManager.Instance.nowReadCharcterID == 1)
            {
                playerMate.mainTexture = yusukeAnimationImages[nowTextureNum];
            }


        }

        else
        {
            if (nowCharacterID == 0)
            {
                playerMate.mainTexture = tatsumiAnimationImages[nowTextureNum];
            }

            if (nowCharacterID == 1)
            {
                playerMate.mainTexture = yusukeAnimationImages[nowTextureNum];
            }


        }

       

    }

    void OnTriggerEnter(Collider other)
    {

        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == "Obstacle")
        {
            SoundManager.Instance.PlaySE("hit");
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        //ステージの移動を止める
        stage.ScrollSpeed = 0.0f;


        hp -= 1;
        DataManager.Instance.minigameHp = hp;
        if (hp > 0)
        {
            //HPの表示を1つ減らす
            hpImage[hp].SetActive(false);
        }
        //HPが0以下
        else
        {
            if (SceneChanger.GetBeforeSceneName() == "MyPage")
            {
                if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 0)
                {
                    face.sprite = tatsumiFaceIconImages[hp];
                }

                if (DataManager.Instance.nowReadCharcterID/*セレクトされたキャラクターID*/ == 1)
                {
                    face.sprite = yusukeFaceIconImages[hp];
                }
            }

            else
            {
                //タツミ
                if (nowCharacterID == 0)
                {
                    face.sprite = tatsumiFaceIconImages[hp];
                }
                //ユウスケ
                if (nowCharacterID == 1)
                {

                    face.sprite = yusukeFaceIconImages[hp];

                }

            }
            hpImage[0].SetActive(false);
            //ステージスクロールをとめる
            stage.ScrollSpeed = 0.0f;
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(1);
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.StopSE();
            SceneChanger.LoadScene("Result");
            //SceneChanger.LoadBeforeScene(true);
        }




        yield return StartCoroutine(DamageEffect());

        //ステージの移動を元に戻す
        stage.ScrollSpeed = stage.roadScrollSpeed;
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
