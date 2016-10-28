using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MyPageManager : MonoBehaviour
{

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

    //前のコメントの配列番号
    int commentPrevLine;



    void Start()
    {

        //キャラクターのセット
        image.texture = texture;

        //キャラクターコメントを読み込む
        //※後で

        //コメントエリアのアルファ値を0して、描画をしなくする
        //ボタン判定は残す
        var col = commentArea.color;
        col.a = commentAreaAlpha;
        commentArea.color = col;

        //0だと配列の０番目と重なるので
        commentPrevLine = -1;

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
