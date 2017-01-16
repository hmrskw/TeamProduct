using UnityEngine;
using System.Collections;

public class PopManager : MonoBehaviour {

    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject menuPopup;

    [SerializeField]
    GameObject choiceCheck;

    [SerializeField]
    GameObject configPopup;

    [SerializeField]
    GameObject backlogPopup;

    [SerializeField]
    ConfigManager configManager;

    public bool isDrawpopup = false;

    private void Update()
    {
        isDrawpopup = (choiceCheck.activeInHierarchy || menuPopup.activeInHierarchy || configPopup.activeInHierarchy || backlogPopup.activeInHierarchy);
        background.SetActive(isDrawpopup);
    }

    public void Menu()
    {
        SoundManager.Instance.PlaySE("main botan");
        menuPopup.SetActive(true);
    }

    public void ChoiceCheck()
    {
        choiceCheck.SetActive(true);
    }

    public void Back()
    {
        SoundManager.Instance.PlaySE("botan");
        isDrawpopup = false;
        menuPopup.SetActive(false);
    }

    public void Mypage()
    {
        if (Fade.Instance.isFade == false)
        {
            DataManager.Instance.endLine = 0;
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.StopSE();
            SoundManager.Instance.PlaySE("botan");
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
        }
    }

    public void Backlog()
    {
        SoundManager.Instance.PlaySE("botan");
        menuPopup.SetActive(false);
        backlogPopup.SetActive(true);
    }

    public void Config()
    {
        //gameObject.SetActive(false);
        SoundManager.Instance.PlaySE("botan");
        menuPopup.SetActive(false);
        configPopup.SetActive(true);

        configManager.PlayText();
    }
}