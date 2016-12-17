using UnityEngine;
using System.Collections;

public class PopManager : MonoBehaviour {

    [SerializeField]
    GameObject configPopup;

    [SerializeField]
    ConfigManager configManager;

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Mypage()
    {
        if (Fade.Instance.isFade == false)
        {
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
        }
    }

    public void Config()
    {
        gameObject.SetActive(false);
        
        configPopup.SetActive(true);

        configManager.PlayText();
    }
}