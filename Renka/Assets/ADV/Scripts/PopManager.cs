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
        menuPopup.SetActive(true);
    }

    public void ChoiceCheck()
    {
        choiceCheck.SetActive(true);
    }

    public void Back()
    {
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
            Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
        }
    }

    public void Backlog()
    {
        menuPopup.SetActive(false);
        backlogPopup.SetActive(true);
    }

    public void Config()
    {
        //gameObject.SetActive(false);
        menuPopup.SetActive(false);
        configPopup.SetActive(true);

        configManager.PlayText();
    }
}