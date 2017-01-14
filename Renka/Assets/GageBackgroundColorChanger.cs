using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GageBackgroundColorChanger : MonoBehaviour {
    [SerializeField]
    Image gageBackground;

    // Use this for initialization
    void Start () {
	    if(DataManager.Instance.masteringData.masteringCharacterID == 0)
        {
            gageBackground.color = new Color(255f/255f, 100f / 255f, 100f / 255f, 100f/255f);
        }
        else if (DataManager.Instance.masteringData.masteringCharacterID == 1)
        {
            gageBackground.color = new Color(100f / 255f, 100f / 255f, 255f / 255f, 100f / 255f);
        }
        else
        {
            gageBackground.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
        }
    }
}
