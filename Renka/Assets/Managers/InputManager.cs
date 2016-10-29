using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    //シーンまたいでもオブジェクトが破棄されなくする
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    /// <summary> 実行環境が Android なら true を返す </summary>
    public bool isAndroid {
        get { return Application.platform == RuntimePlatform.Android; }
    }

    /// <summary> 実行環境が iOS なら true を返す </summary>
    public bool isIPhone {
        get { return Application.platform == RuntimePlatform.IPhonePlayer; }
    }

    /// <summary> 実行環境がスマートフォンなら true を返す </summary>
    public bool isSmartDevice {
        get { return isAndroid || isIPhone; }
    }

    /// <summary> タッチされたときのスクリーン座標を返す：左下 (0, 0) </summary>
    public Vector3 GetTouchPosition() {
        // Vector3 として扱うのは、オブジェクトの transform.position が Vector3 のため
        Vector3 touch = Vector3.zero;

#if UNITY_STANDALONE
        // スマートフォンでなければ、マウス座標を代わりに返す
        touch = Input.mousePosition;
#else
    // タッチされていれば、スクリーンの座標を返す
    if (Input.touchCount > 0) { touch = Input.touches[0].position; }
#endif

        return touch;
    }

    // タッチが指定された状態に一致するなら true を返す
    bool GetTouchState(TouchPhase state) {
        return Input.touchCount > 0 ?
          Input.touches[0].phase == state : false;
    }

    /// <summary> タッチされたら true を返す </summary>
    public bool IsTouchBegan() {
        bool result = false;
#if UNITY_STANDALONE
        // Unity エディター、または Windows、MacOSX 向けビルドの場合の判定
        // マウスの左クリックを判定基準にする
        result = Input.GetMouseButtonDown(0);
#else
    // タッチされていれば、その状態を調べる
    result = GetTouchState(TouchPhase.Began);
#endif
        return result;
    }

    /// <summary> タッチされ続けている間 true を返す </summary>
    public bool IsTouchMoved() {
        bool result = false;

#if UNITY_STANDALONE
        result = Input.GetMouseButton(0);
#else
    // タッチされていれば、その状態を調べる
    if (Input.touchCount > 0)
    {
      var touch = Input.touches[0];

      // 動いていないがタッチされ続けている場合もあるので、それも考慮する
      bool isMove = (touch.phase == TouchPhase.Moved);
      bool isWait = (touch.phase == TouchPhase.Stationary);

      result = isMove || isWait;
    }
#endif
        return result;
    }

    /// <summary> タッチが離されたら true を返す </summary>
    public bool IsTouchEnded() {
        bool result = false;

#if UNITY_STANDALONE
        result = Input.GetMouseButtonUp(0);
#else
    result = GetTouchState(TouchPhase.Ended);
#endif
        return result;
    }

    /// <summary> 端末の戻るボタンが押されたら true を返す </summary>
    public bool IsPushedQuitKey() {
        // KeyCode.Escape は、スマートフォンの戻るボタンに対応している
        return Input.GetKeyDown(KeyCode.Escape);
    }
}