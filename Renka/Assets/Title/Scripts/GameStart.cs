using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    bool canStart;

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBGM("title");
        int characterID = Random.Range(0, 2);
        if (characterID == 0)
        {
            SoundManager.Instance.PlayVoice("tatsumi_1");
        }
        else
        {
            SoundManager.Instance.PlayVoice("yusuke_1");
        }
        StartCoroutine(PlayGame());
	}
	
	// Update is called once per frame
	IEnumerator PlayGame () {
        while (true)
        {
            if (SoundManager.Instance.isPlayVoice() == false && InputManager.Instance.IsTouchEnded()) break;
            yield return null;
        }

        SoundManager.Instance.PlaySE("botan");
        if (DataManager.Instance.masteringData.masteringCharacterID == -1)
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("ADV"); });
        else
            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MyPage"); });
	}
}
