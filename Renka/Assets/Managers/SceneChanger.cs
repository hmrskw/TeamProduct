using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public delegate void Method();

    static Stack<string> beforeSceneName = new Stack<string>();

    /// <summary>移動前のシーン名を保存してシーンを移動</summary>
    /// <param name="sceneName">移動先のシーン名</param>
    public static void LoadScene(string sceneName, bool isPushSceneName = false)
    {
        if(isPushSceneName)
            beforeSceneName.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>一つ前のシーンに移動</summary>
    public static void LoadBeforeScene(bool isPushSceneName = false)
    {
        if (GetBeforeSceneName() != SceneManager.GetActiveScene().name)
        {
            if (beforeSceneName != null)
            {
                string sceneNameTmp = SceneManager.GetActiveScene().name;
                string beforeTmp = beforeSceneName.Pop();
                if (isPushSceneName)
                    beforeSceneName.Push(sceneNameTmp);
                SceneManager.LoadScene(beforeTmp);
            }
            else
            {
                Debug.LogError("前回のシーンが保存されていません");
                SceneManager.LoadScene("MyPage");
            }
        }
    }

    /// <summary>一つ前のシーン名を取得</summary>
    /// <returns>一つ前のシーン名</returns>
    public static string GetBeforeSceneName(bool isPopSceneName = false)
    {
        if (beforeSceneName.Count > 0) {
            if (isPopSceneName)
                return beforeSceneName.Pop();
            else
                return beforeSceneName.Peek();
        }
        else {
            return null;
        }
        
    }

    public static void ResetBeforeScene()
    {
        Debug.Log("clear");
        beforeSceneName.Clear();
    }
}