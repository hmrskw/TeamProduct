using UnityEngine;
using System.Collections;
using live2d; //Live2Dライブラリを使用

public class Live2DCharacterMove : MonoBehaviour
{

    //キャラごとの見た目のバリエーション
    [System.Serializable]
    public struct Live2DData
    {
        public TextAsset mocFile; // mocファイル
        public Texture2D texture; // テクスチャファイル    
        //表情画像の配列
        public TextAsset[] mtnFiles;
    }
    public Live2DData[] live2DData; // mtnファイル

    private Live2DModelUnity live2DModel; //Live2Dモデルクラス
    private Live2DMotion motion; // Live2Dモーションクラス
    private MotionQueueManager motionManager; // モーション管理クラス

    int charID;
    void Start()
    {
        if (SceneChanger.GetBeforeSceneName() == "MyPage")
        {
            Debug.Log("mypage");
            //タツミ
            charID = DataManager.Instance.nowReadCharcterID;
        }

        //前のシーンがマイページ以外なら
        else
        {
            charID = DataManager.Instance.masteringData.masteringCharacterID;
        }

        int resultID = 0;
        if (DataManager.Instance.minigameHp == 3)
        {
            resultID = 0;
        }
        else if (DataManager.Instance.minigameHp <= 0)
        {
            resultID = 2;
        }
        else
        {
            resultID = 1;
        }


        //初期化（Live2Dを使用する前に必ず１度だけ呼び出す）
        Live2D.init();
        //モデルデータを読み込む
        live2DModel = Live2DModelUnity.loadModel(live2DData[charID].mocFile.bytes);
        //テクスチャの関連付け
        live2DModel.setTexture(0, live2DData[charID].texture);

        // モーションのインスタンスの作成（mtnの読み込み）と設定
        motion = Live2DMotion.loadMotion(live2DData[charID].mtnFiles[resultID].bytes);
        motion.setLoop(true);

        // モーション管理クラスのインスタンスの作成
        motionManager = new MotionQueueManager();
        // モーションの再生
        motionManager.startMotion(motion, false);
    }

    // デフォルトでは DrawMeshNow でLive2Dモデルを描画するので OnRenderObject を使う
    void OnRenderObject()
    {
        //表示位置と大きさの指定
        Matrix4x4 m1 = Matrix4x4.Ortho(0, 900, 900, 0, -0.5f, 0.5f);
        Matrix4x4 m2 = transform.localToWorldMatrix;
        Matrix4x4 m3 = m2 * m1;
        live2DModel.setMatrix(m3);

        // 再生中のモーションからモデルパラメータを更新
        motionManager.updateParam(live2DModel);

        //頂点の更新
        live2DModel.update();

        //モデルの描画
        live2DModel.draw();
    }
}