using UnityEngine;
using System.Collections;

public class ChengeLighting : MonoBehaviour {
    [SerializeField]
    Material skyMorning;
    [SerializeField]
    Material skyNight;

    Color nightColor = new Color(0.2f,0.2f,0.17f);
    Color MorningColor = new Color(0.49f, 0.54f, 0.61f);
    // Use this for initialization
    void Start () {
        if(DataManager.Instance.difficulty == 2)
        {
            RenderSettings.skybox = skyNight;
            RenderSettings.ambientSkyColor = nightColor;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
       
	}
}
