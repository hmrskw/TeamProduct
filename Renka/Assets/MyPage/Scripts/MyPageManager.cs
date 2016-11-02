using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MyPageManager : MonoBehaviour
{

    [System.Serializable]
    public struct CharacterVisualVariation_
    {
        //体の画像
        public Texture bodyTex;

        //表情画像の配列
        public Texture[] faceTexs;

        //服装画像の配列
        public Texture[] clothesTexs;
    }

    [SerializeField, Tooltip("コメントを表示させるためのタップ範囲のオブジェ")]
    Image commentArea;

    [SerializeField, Tooltip("上のアルファ値")]
    float commentAreaAlpha;

    [SerializeField, Tooltip("キャラクターのコメント")]
    string[] comments;

    [SerializeField, Tooltip("コメントを表示するText")]
    Text commentText;

    [SerializeField, Tooltip("攻略中のキャラクターのテクスチャ")]
    Texture texture;

    [SerializeField, Tooltip("キャラクターを描画するイメージの参照")]
    RawImage image;

    [SerializeField, Tooltip("キャラクターの顔を描画するイメージの参照")]
    RawImage faceImage;

    [SerializeField, Tooltip("キャラクターの服装を描画するイメージの参照")]
    RawImage clothesImage;

    [SerializeField, Tooltip("表示するキャラのトランスフォーム")]
    RectTransform rectTrans;

    //前のコメントの配列番号
    int commentPrevLine;

    [SerializeField, Tooltip("好感度ゲージ")]
    Slider gauge;

    [SerializeField, Tooltip("好感度の最大値")]
    int likeMax;

    [SerializeField, Tooltip("描画するかもしれないキャラのデータ")]
    CharacterVisualVariation_[] charVariations;

    void Start()
    {

        //コメントエリアのアルファ値を0して、描画をしなくする
        //ボタン判定は残す
        var col = commentArea.color;
        col.a = commentAreaAlpha;
        commentArea.color = col;

        //0だと配列の０番目と重なるので
        commentPrevLine = -1;

        //好感度の最大値と現在の好感度からゲージの割合を決める
        Debug.Log( "好感度 : " + DataManager.Instance.likeabillity);
        gauge.value = (float)DataManager.Instance.likeabillity / (float)likeMax;

        //キャラクターの生成
        var i = DataManager.Instance.masteringCharacterID;
        if (i >= 0 && i < 4)
        {
            //キャラのセット
            image.texture = charVariations[i].bodyTex;
            Debug.Log( charVariations[i].bodyTex );
            faceImage.texture = charVariations[i].faceTexs[0];
            clothesImage.texture = charVariations[i].clothesTexs[0];
            Debug.Log("キャラ生成 : " + i);
        }
        else
        {
            //シャア
            image.texture = texture;
            faceImage.enabled = false;
            clothesImage.enabled = false;
            rectTrans.anchoredPosition = new Vector2(0, 200);
            rectTrans.localScale = new Vector3(0.75f, 0.6f, 1);
            Debug.Log("シャアが来る");
        }

    }

    /// <summary>
    /// Storyボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickStory()
    {
        //Debug.Log(DataManager.Instance.endLine);
        SceneManager.LoadScene("ADV");
    }

    /// <summary>
    /// MiniGameボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickMiniGame()
    {
        SceneManager.LoadScene("MiniGame");
        //Debug.Log("Click MiniGame");
    }

    /// <summary>
    /// Galleryボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickGallery()
    {
        Debug.Log("Click Gallery");
    }

    /// <summary>
    /// Configボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickConfig()
    {
        Debug.Log("Click Config");
    }

    /// <summary>
    /// CommentAreaが押されたときに呼ばれる
    /// </summary>
    public void OnClickCommentArea()
    {
        Debug.Log("Click CommentArea");

        //ランダムにコメントを変える
        var line = Random.Range(0, comments.Length);

        //同じコメントを呼ばない
        if (comments.Length != 0 && comments.Length != 1)
        {
            while (line == commentPrevLine)
            {
                Debug.Log("Re: Randam : " + line);
                line = Random.Range(0, comments.Length);
            }
            commentPrevLine = line;
        }

        //コメントを表示テキストに上書きする
        commentText.text = comments[line];
        Debug.Log("Randam Comment : " + line);
    }

}
