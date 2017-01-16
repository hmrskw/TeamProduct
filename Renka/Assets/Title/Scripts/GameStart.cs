using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBGM("title");
        StartCoroutine(PlayGame());
	}
	
	// Update is called once per frame
	IEnumerator PlayGame () {
        while (true)
        {
            if (InputManager.Instance.IsTouchEnded()) break;
            yield return null;
        }

        SoundManager.Instance.PlaySE("botan");
        if (DataManager.Instance.masteringData.masteringCharacterID == -1)
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("ADV"); });
        else
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MyPage"); });
	}
}
