using UnityEngine;
using System.Collections;
using live2d; //Live2Dライブラリを使用

public class main : MonoBehaviour
{

    public TextAsset mocFile; // mocファイル
    public Texture2D texture; // テクスチャファイル
    public TextAsset[] mtnFiles; // mtnファイル

    private Live2DModelUnity live2DModel; //Live2Dモデルクラス
    private Live2DMotion motion; // Live2Dモーションクラス
    private MotionQueueManager motionManager; // モーション管理クラス

    void Start()
    {
        //初期化（Live2Dを使用する前に必ず１度だけ呼び出す）
        Live2D.init();
        //モデルデータを読み込む
        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
        //テクスチャの関連付け
        live2DModel.setTexture(0, texture);

        // モーションのインスタンスの作成（mtnの読み込み）と設定
        motion = Live2DMotion.loadMotion(mtnFiles[0].bytes);
        motion.setLoop(true);

        // モーション管理クラスのインスタンスの作成
        motionManager = new MotionQueueManager();
        // モーションの再生
        motionManager.startMotion(motion, false);
    }

    void Update()
    {

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