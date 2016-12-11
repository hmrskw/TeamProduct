using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GraphicManager : MonoBehaviour
{

    //キャラごとの見た目のバリエーション
    [System.Serializable]
    public struct CharacterVisualVariation_
    {
        //体の画像
        public Texture bodyTex;

        //表情画像の配列
        public Texture[] faceTexs;

        //服装画像の配列
        //public Texture[] clothesTexs;
    }

    //描画するための器
    public class Character_
    {
        //ゲームオブジェクト
        public GameObject obj;

        //アクセス用の変数
        public Character charScript;

        //全体の座標
        public RectTransform rectTrans;

        //描画される体の画像の参照先
        public RawImage body;

        //描画される表情の画像の参照先
        public RawImage face;

        //描画される服装の画像の参照先
        //public RawImage clothes;

        //描画される体の画像の参照先
        public int CharacterNumber;

        //表情の番号
        public int faceNumber;

        //服装の番号
        //public int clothesNumber;

        //座標データ...zでサイズ指定
        public Vector2 pos;

        public float size;

        //表情調整用...zでサイズ指定
        public Vector2 faceVec;

        //服装調整用...zでサイズ指定
        public Vector2 clothesVec;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Setup()
        {
            CharacterNumber = 0;
            faceNumber = 0;
            //clothesNumber = 0;

            pos = new Vector2(0, 0);
            faceVec = Vector2.zero;
            clothesVec = Vector2.zero;
            rectTrans = charScript.GetComponent<RectTransform>();
            body = charScript.GetComponent<RawImage>();
            face = charScript.face.GetComponent<RawImage>();
            //clothes = charScript.clothes.GetComponent<RawImage>();

            ChangeTexs();
        }

        /// <summary>
        /// 上書きされた番号に変える
        /// </summary>
        public void ChangeTexs()
        {
            //RawImaget.textureを変更する
            body.texture = characterVariationsBuffer[CharacterNumber].bodyTex;
            face.texture = characterVariationsBuffer[CharacterNumber].faceTexs[faceNumber];
            //face.texture = characterVariationsBuffer[CharacterNumber].faceTexs[faceNumber];
            //clothes.texture = characterVariationsBuffer[CharacterNumber].clothesTexs[clothesNumber];            
        }

        /// <summary>
        /// 顔の画像を上書きされた番号に変える
        /// </summary>
        public void ChangeFace()
        {
            //face
            face.texture = characterVariationsBuffer[0].faceTexs[faceNumber];
        }

        /// <summary>
        /// 服装の画像を上書きされた番号に変える
        /// </summary>
        /*public void ChangeClothes()
        {
            Debug.Log(clothesNumber);
            clothes.texture = characterVariationsBuffer[0].clothesTexs[clothesNumber];
        }*/

        //描画フラグ
        bool isDraws;
    }

    //キャラデータ...要素数 == キャラクターの種類
    [SerializeField, Tooltip("キャラクター、顔、服装の種類すべてを設定する"),Header("キャラクター")]
    CharacterVisualVariation_[] characterVariations;

    //charcters[]からアクセスする用...上を参照させる
    static CharacterVisualVariation_[] characterVariationsBuffer;

    //描画用のキャラデータ...要素数 == 最大表示数
    Character_[] characters;

    [SerializeField, Range(1, 4), Tooltip("キャラクターの最大表示数&インスタンス化数")]
    int characterMax;

    //最大キャラ表示数に応じて複製する場所
    [SerializeField, Tooltip("キャラクター(複数)のプレハブのインスタンス化先")]
    GameObject charactersObject;

    //キャラクターのプレハブ
    [SerializeField, Tooltip("インスタンス化されるキャラクターのオリジン")]
    GameObject charPrefabObj;

    [Space(15), Header("背景")]

    //背景データ
    [SerializeField, Tooltip("背景")]
    Texture[] backgroundTexs;

    //描画される背景の参照用...textureを入れ替えると背景が変わる
    [SerializeField, Tooltip("描画される背景の参照")]
    RawImage background;

    //現在の背景番号
    int backgroundCurrent;

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    GameObject intermission;

    void Start()
    {
        //インスペクターで設定したキャラバリエーションを別クラスでアクセスするため
        characterVariationsBuffer = characterVariations;

        //描画に必要な数のキャラを生成
        characters = new Character_[characterMax];
        for (int i = 0; i < characterMax; ++i)
        {
            var trans = charactersObject.transform;
            characters[i] = new Character_();
            characters[i].obj = (GameObject)Instantiate(charPrefabObj, trans);

            //サイズが1.0倍にならないので
            characters[i].obj.transform.localScale = Vector3.one;

            //座標が原点になるで指定
            characters[i].obj.transform.localPosition = new Vector2(0, 0);

            //体パーツへの参照スクリプト
            characters[i].charScript = characters[i].obj.GetComponent<Character>();

            //初期化
            characters[i].Setup();
        }
        DrawCharacter(ConvertADVdata.Instance.advData);
        DrawBack("黒");
    }

    /// <summary>
    /// キャラクターを描画
    /// </summary>
    /// <param name="csv_">今読んでいるCSVのデータ</param>
    public void DrawCharacter(List<ConvertADVdata.ADVData> csv_)
    {
        for (int i = 0; i < csv_[DataManager.Instance.endLine].drawCharacterNum; i++)
        {
            characters[i].CharacterNumber = csv_[DataManager.Instance.endLine].drawCharacterID[i];

            characters[i].faceNumber = csv_[DataManager.Instance.endLine].expression[i];

            //characters[i].clothesNumber = csv_[DataManager.Instance.endLine].costume[i];
            
            //Debug.Log(csv_[DataManager.Instance.endLine].pos[i]);
            characters[i].rectTrans.anchoredPosition = csv_[DataManager.Instance.endLine].pos[i];

            var size = csv_[DataManager.Instance.endLine].size[i];
            Debug.Log(csv_[DataManager.Instance.endLine].size[i]);
            //characters[i].rectTrans.sizeDelta = new Vector2(size, size);
            characters[i].rectTrans.localScale = new Vector3(size, size, 1);

            characters[i].ChangeTexs();
            characters[i].obj.SetActive(true);
        }
    }

    IEnumerator ChangeBack(Texture tmp)
    {
        while (Fade.Instance.isFade)
        {
            yield return null;
        }
        background.texture = tmp;
    }
    public void DrawBack(string backGround_)
    {
        Texture tmp = backgroundTexs[BackgroundTextureNameToID(backGround_)];
        if (background.texture != tmp) {
            StartCoroutine(ChangeBack(tmp));
            //background.texture = tmp;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].obj.SetActive(false);
        }
    }

    public void ChangeCanvasNext()
    {
        canvas.SetActive(false);
        intermission.SetActive(true);
    }

    /// <summary> 背景名から各背景に割り振られているIDに変換 </summary>
    /// <param name="backgroundTextureName_">背景名</param>
    /// <returns>背景に割り振られたID</returns>
    int BackgroundTextureNameToID(string backgroundTextureName_)
    {
        int id = 0;
        if (backgroundTextureName_ == "白")
        {
            id = 0;
        }
        if (backgroundTextureName_ == "黒")
        {
            id = 1;
        }
        if (backgroundTextureName_ == "町")
        {
            id = 2;
        }
        if (backgroundTextureName_ == "部屋")
        {
            id = 3;
        }
        if (backgroundTextureName_ == "火事")
        {
            id = 4;
        }
        return id;
    }
}
