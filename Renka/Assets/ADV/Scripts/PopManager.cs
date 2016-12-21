using UnityEngine;
using System.Collections;

public class PopManager : MonoBehaviour {

    [SerializeField]
    GameObject configPopup;

    [SerializeField]
    ConfigManager configManager;

    bool isDrawpopup;

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Mypage()
    {
        if (Fade.Instance.isFade == false)
        {
            DataManager.Instance.endLine = 0;
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
        }
    }

    public void Config()
    {
        //gameObject.SetActive(false);
        
        configPopup.SetActive(true);

        configManager.PlayText();
    }
}