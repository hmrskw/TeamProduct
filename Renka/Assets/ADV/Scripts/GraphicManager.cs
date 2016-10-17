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
        //名前と番号...どちらか消してもよい
        public string name;
        public int id;

        //体の画像
        public Texture bodyTex;

        //表情画像の配列
        public Texture[] faceTexs;

        //服装画像の配列
        public Texture[] clothesTexs;
    }

    //描画するための器
    public class Character_
    {
        //ゲームオブジェクト
        public GameObject obj;

        //アクセス用の変数?
        public Character charScript;

        //描画される体の画像の参照先
        public RawImage body;

        //描画される表情の画像の参照先
        public RawImage face;

        //描画される服装の画像の参照先
        public RawImage clothes;


        //名前とID番号
        string name;
        public int id;

        //表情の番号
        public int faceNumber;

        //服装の番号
        public int clothesNumber;

        //座標データ...zでサイズ指定
        Vector3 pos;

        //表情調整用...zでサイズ指定
        Vector3 faceVec;

        //服装調整用...zでサイズ指定
        Vector3 clothesVec;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Setup()
        {
            name = "名前";
            id = 0;
            faceNumber = 0;
            clothesNumber = 0;
            pos = new Vector3(0,0,0);
            faceVec = Vector3.zero;
            clothesVec = Vector3.zero;
            body = charScript.GetComponent<RawImage>();
            face = charScript.face.GetComponent<RawImage>();
            clothes = charScript.clothes.GetComponent<RawImage>();

            ChangeTexs();
        }

        /// <summary>
        /// 上書きされた番号に変える
        /// </summary>
        public void ChangeTexs()
        {
            //RawImaget.textureを変更する
            //charScript.GetComponent<RawImage>().texture = characterVariationsBuffer[id].bodyTex;
            //charScript.face.GetComponent<RawImage>().texture = characterVariationsBuffer[id].faceTexs[faceNumber];
            //charScript.clothes.GetComponent<RawImage>().texture = characterVariationsBuffer[id].clothesTexs[clothesNumber];
            body.texture = characterVariationsBuffer[id].bodyTex;
            face.texture = characterVariationsBuffer[id].faceTexs[faceNumber];
            clothes.texture = characterVariationsBuffer[id].clothesTexs[clothesNumber];

        }

        /// <summary>
        /// 顔の画像を上書きされた番号に変える
        /// </summary>
        public void ChangeFace()
        {
            face.texture = characterVariationsBuffer[id].faceTexs[faceNumber];
        }

        /// <summary>
        /// 服装の画像を上書きされた番号に変える
        /// </summary>
        public void ChangeClothes()
        {
            clothes.texture = characterVariationsBuffer[id].clothesTexs[clothesNumber];
        }

        //描画フラグ
        bool isDraws;
    }

    //キャラデータ...要素数 == キャラクターの種類
    [SerializeField, Tooltip("キャラクター、顔、服装の種類すべてを設定する")]
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

    //背景データ
    [SerializeField, Tooltip("背景")]
    Texture[] backgroundTexs;

    //描画される背景の参照用...textureを入れ替えると背景が変わる
    [SerializeField, Tooltip("描画される背景の参照")]
    RawImage background;

    //現在の背景番号
    int backgroundCurrent;


    void Start ()
    {

        //インスペクターで設定したキャラバリエーションを別クラスでアクセスするため
        characterVariationsBuffer = characterVariations;

        //var obj = FindObjectOfType<>();

        //描画に必要な数のキャラを生成
        characters = new Character_[characterMax];
        for (int i = 0; i < characterMax; ++i)
        {
            //charactersObject.transform.Rotate(3,3,3);
            var trans = charactersObject.transform;
            characters[i] = new Character_();
            characters[i].obj = (GameObject)Instantiate(charPrefabObj, trans);

            //サイズが1.0倍にならないので
            characters[i].obj.transform.localScale = Vector3.one;

            //座標が原点になるで指定
            characters[i].obj.transform.localPosition  = new Vector3(0, 0, 0);
            
            //体パーツへの参照スクリプト
            characters[i].charScript = characters[i].obj.GetComponent<Character>();
            
            //初期化
            characters[i].Setup();
        }

    }

	void Update ()
    {
        //テスト
        UpdateTest();
    }

    //テスト関数...画像が差し変わる
    void UpdateTest()
    {

        //(´・ω・`)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ++characters[0].faceNumber;
            if (characters[0].faceNumber > characterVariationsBuffer[characters[0].id].faceTexs.Length-1) characters[0].faceNumber = 0;
            characters[0].ChangeFace();
        }

        //服装変える
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++characters[0].clothesNumber;
            if (characters[0].clothesNumber > characterVariationsBuffer[characters[0].id].clothesTexs.Length - 1) characters[0].clothesNumber = 0;
            characters[0].ChangeClothes();
        }

        //1,2,3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            background.texture = backgroundTexs[1 - 1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            background.texture = backgroundTexs[2 - 1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            background.texture = backgroundTexs[3 - 1];
        }

    }



}
