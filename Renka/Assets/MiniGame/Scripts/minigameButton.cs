using UnityEngine;
using System.Collections;

public class minigameButton : MonoBehaviour {
  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonSE()
    {
        SoundManager.Instance.PlaySE("botan");
    }

    public void ChangeScene(string sceneName)
    {
        SceneChanger.LoadScene(sceneName);
    }
}
