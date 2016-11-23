using UnityEngine;
using System.Collections.Generic;

public class DebugUI : MonoBehaviour {
    private const int LOG_MAX = 10;
    private Queue<string> logStack = new Queue<string>(LOG_MAX);

    void Awake()
    {
        Application.logMessageReceived += LogCallback;  // ログが書き出された時のコールバック設定

        //Debug.LogWarning(Application.streamingAssetsPath + "/CSVFiles/pathName");   // テストでワーニングログをコール
        string path = Application.persistentDataPath + "/";
        string fileName = Application.companyName + "." + Application.productName + ".savedata.json";
        Debug.LogWarning(path + fileName);   // テストでワーニングログをコール
    }

    /// <summary>
    /// ログを取得するコールバック
    /// </summary>
    /// <param name="condition">メッセージ</param>
    /// <param name="stackTrace">コールスタック</param>
    /// <param name="type">ログの種類</param>
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        // 通常ログまで表示すると邪魔なので無視
        if (type == LogType.Log)
            return;

        string trace = null;
        string color = null;

        switch (type)
        {
            case LogType.Warning:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "yellow";
                break;
            case LogType.Error:
            case LogType.Assert:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "red";
                break;
            case LogType.Exception:
                trace = stackTrace;
                color = "red";
                break;
        }

        // ログの行制限
        //if (this.logStack.Count == LOG_MAX)
        //    this.logStack.Dequeue();

        //        string message = string.Format("<color={0}>{1}</color> <color=white>on {2}</color>", color, condition, trace);
        string message = string.Format("<color={0}>{1}</color>", color, condition);
        this.logStack.Enqueue(message);
    }

    /// <summary>
    /// エラーログ表示
    /// </summary>
    void OnGUI()
    {
        if (this.logStack == null || this.logStack.Count == 0)
            return;

        // 表示領域は任意
        float space = 16f;
        float height = 500f;
        Rect drawArea = new Rect(space, 0f + space, (float)Screen.width * 0.8f, height);
        GUI.Box(drawArea, "");

        GUILayout.BeginArea(drawArea);
        {
            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            foreach (string log in logStack)
                GUILayout.Label(log, style);
        }
        GUILayout.EndArea();
    }
}