using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BacklogCreator : MonoBehaviour
{
    [SerializeField]
    GameObject popup;

    [SerializeField]
    Transform parentTransform;

    [SerializeField]
    RectTransform prefab = null;

    [SerializeField]
    TextManager textManager;

    int logCount = 0;

    void OnEnable()
    {
        for (int i = logCount; i < textManager.logArray.Count; i++)
        {
            
            RectTransform item = Instantiate(prefab);

            item.SetParent(parentTransform, false);

            item.GetComponent<BacklogView>().ViewBacklogText(textManager.logArray[i].name,textManager.logArray[i].text);
            //var text = item.GetComponentInChildren<Text>();
            //text.text = textManager.logArray[i].text;
        }
        logCount = textManager.logArray.Count;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        popup.SetActive(true);
    }

}