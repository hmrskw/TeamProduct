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

    [SerializeField]
    private GameObject faceIconUi;

    private Image face;

    private bool is_hit = false;

    void Start()
    {
        hp = hpImage.Length;
        face = faceIconUi.GetComponent<Image>();
        

    }

   void Update()
    {

        if(hp>0)
        {
            face.sprite = faceIconImages[hp-1];

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
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
            hpImage[0].SetActive(false);
            //ステージスクロールをとめる
            stage.ScrollSpeed = 0.0f;
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("MyPage");
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

            //renderer.material.color = new Color(1f, 0, 0, sin);
            var color = Color.red;
            color.a = alpha;
            renderer.material.color = color;

            yield return null;
          
        }

        renderer.material.color = Color.red;
    }
}
