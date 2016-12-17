using UnityEngine;
using System.Collections;

public class ConfigView : MonoBehaviour {

    [SerializeField]
    GameObject popup;

    [SerializeField]
    TextManager textManager;

    ConfigManager configManager;

    void Start()
    {
        configManager = GetComponent<ConfigManager>();
    }
    public void Back()
    {
        gameObject.SetActive(false);
        configManager.SaveConfigData();
        textManager.SetTextAreaColor();
        popup.SetActive(true);
    }
}
