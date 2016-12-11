using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MyPageManager : MonoBehaviour
{

    [System.Serializable]
    public struct CharacterVisualVariation_
    {
        //体の画像
        //public Texture bodyTex;

        //表情画像の配列
        public Texture[] faceTexs;

        //服装画像の配列
        //public Texture[] clothesTexs;
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

    [SerializeField]
    Button storyButton;

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
        //Debug.Log(SceneChanger.GetBeforeSceneName());
        Fade.Instance.FadeOut(1f, null);

        if (DataManager.Instance.isEndChapter() && DataManager.Instance.isEndStory())
        {
            storyButton.interactable = false;
        }

        SceneChanger.ResetBeforeScene();

        //コメントエリアのアルファ値を0して、描画をしなくする
        //ボタン判定は残す
        var col = commentArea.color;
        col.a = commentAreaAlpha;
        commentArea.color = col;

        //0だと配列の０番目と重なるので
        commentPrevLine = -1;

        //好感度の最大値と現在の好感度からゲージの割合を決める
        gauge.value = (float)DataManager.Instance.masteringData.likeabillity / (float)likeMax;

        //キャラクターの生成
        var i = DataManager.Instance.masteringData.masteringCharacterID;
        if (i >= 0 && i < 4)
        {
            //キャラのセット
            //image.texture = charVariations[i].bodyTex;
            faceImage.gameObject.SetActive(false);
            //clothesImage.texture = charVariations[i].clothesTexs[0];
            rectTrans.anchoredPosition = new Vector3(0, -200, 0);
            image.texture = charVariations[i].faceTexs[0];
        }
        else
        {
            //辰己
            faceImage.gameObject.SetActive(false);
            rectTrans.anchoredPosition = new Vector3(0, -200, 0);
            image.texture = charVariations[0].faceTexs[0];
        }

        var line = Random.Range(0, comments.Length);
        commentText.text = comments[line];
    }
    
    /// <summary>
    /// Storyボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickStory()
    {
        //Debug.Log(DataManager.Instance.endLine);
        Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("ADV"); });

        //SceneChanger.LoadScene("ADV");
    }

    /// <summary>
    /// MiniGameボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickMiniGame()
    {
        Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MiniGame", true); });

        //SceneChanger.LoadScene("MiniGame",true);
    }

    /// <summary>
    /// Galleryボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickGallery()
    {
        //SaveData.ResetMasteringData();
        //DataManager.Instance.Init();
        Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("Menu"); });
        //SceneChanger.LoadScene("Menu");
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
        //ランダムにコメントを変える
        var line = Random.Range(0, comments.Length);

        //同じコメントを呼ばない
        if (comments.Length != 0 && comments.Length != 1)
        {
            while (line == commentPrevLine)
            {
                line = Random.Range(0, comments.Length);
            }
            commentPrevLine = line;
        }

        //コメントを表示テキストに上書きする
        commentText.text = comments[line];
    }
}
