using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    Dictionary<string, AudioClip> seDict = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> voiceDict = new Dictionary<string, AudioClip>();

    [SerializeField]
    AudioSource[] seSource;

    [SerializeField]
    AudioSource bgmSource;

    [SerializeField]
    AudioSource voiceSource;

    /*[System.Serializable]
    public class Clips
    {
        public AudioClip[] seClips;
        public AudioClip[] bgmClips;
    }*/

    [SerializeField]
    AudioClip[] seClips;

    [SerializeField]
    AudioClip[] bgmClips;

    [SerializeField]
    AudioClip[] voiceClips;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        foreach(AudioClip clip in seClips)
        {
            seDict.Add(clip.name, clip);
        }
        foreach (AudioClip clip in bgmClips)
        {
            bgmDict.Add(clip.name, clip);
        }
        foreach (AudioClip clip in voiceClips)
        {
            voiceDict.Add(clip.name, clip);
        }
    }

    void Start()
    {
        ChangeSEVolume(DataManager.Instance.configData.se);
        ChangeBGMVolume(DataManager.Instance.configData.bgm);
    }

    /// <summary>
    /// SEの音量を変更する
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeSEVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);

        foreach (AudioSource source in seSource)
        {
            source.volume = volume;
            DataManager.Instance.configData.se = volume;
        }
    }

    /// <summary>
    /// SEの音量を取得
    /// </summary>
    /// <returns>SEの音量</returns>
    public float GetSEVolume()
    {
        return seSource[0].volume;
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="seName">再生したいSEのファイル名</param>
    public void PlaySE(string seName)
    {
        //今回使うseSourceの番号
        //int i = 0;

        //使用していないseSourceを探す
        for (int i = 0; i < seSource.Length; i++)
        {
            if (seSource[i].clip != null && seSource[i].clip.name == seName)
            {
                seSource[i].Play();
                return;
            }
            else if (seSource[i].isPlaying == false)
            {
                seSource[i].clip = seDict[seName];
                seSource[i].Play();
                return;
            }
        }
        Debug.LogWarning("同時に再生できる音の数を超えたので鳴らせませんでした。");
    }

    /// <summary>
    /// SEを停止
    /// </summary>
    /// <param name="isPause">一時停止か</param>
    public void StopSE(bool isPause = true)
    {
        if (isPause == false)
        {
            foreach (AudioSource source in seSource)
            {
                source.Stop();
            }
        }
        else
        {
            foreach (AudioSource source in seSource)
            {
                source.Pause();
            }
        }
    }

    /// <summary>
    /// BGMの音量を変更する
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeBGMVolume(float volume)
    {
        bgmSource.volume = volume/2f;
        DataManager.Instance.configData.bgm = volume;
    }

    /// <summary>
    /// BGMの音量を取得
    /// </summary>
    /// <returns>BGMの音量</returns>
    public float GetBGMVolume()
    {
        return bgmSource.volume;
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="bgmName">再生したいBGM名</param>
    public void PlayBGM(string bgmName)
    {
        //if (bgmSource.isPlaying) bgmSource.Stop();
        if(bgmSource.clip != bgmDict[bgmName])
            bgmSource.clip = bgmDict[bgmName];

        bgmSource.Play();
    }

    /// <summary>
    /// BGMを停止
    /// </summary>
    /// <param name="isPause">一時停止か</param>
    public void StopBGM(bool isPause = true)
    {
        if (isPause == false) bgmSource.Stop();
        else
        {
            Debug.Log("Pause");
            bgmSource.Pause();
        }
    }

    /// <summary>
    /// ボイスの音量を変更する
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeVoiceVolume(float volume)
    {
        voiceSource.volume = volume;
        DataManager.Instance.configData.voice = volume;
    }

    /// <summary>
    /// ボイスの音量を取得
    /// </summary>
    /// <returns>BGMの音量</returns>
    public float GetVoiceVolume()
    {
        return voiceSource.volume;
    }

    /// <summary>
    /// ボイスを再生する
    /// </summary>
    /// <param name="bgmName">再生したいBGM名</param>
    public void PlayVoice(string voiceName)
    {
        //if (bgmSource.isPlaying) bgmSource.Stop();
        if (voiceSource.clip != voiceDict[voiceName])
            voiceSource.clip = voiceDict[voiceName];
        voiceSource.Play();
    }

    /// <summary>
    /// ボイスを停止
    /// </summary>
    /// <param name="isPause">一時停止か</param>
    public void StopVoice(bool isPause = true)
    {
        if (isPause == false) voiceSource.Stop();
        else
        {
            Debug.Log("Pause");
            voiceSource.Pause();
        }
    }

    /// <summary>
    /// ボイスを再生中か
    /// </summary>
    /// <returns></returns>
    public bool isPlayVoice()
    {
        return voiceSource.isPlaying;
    }

    public string GetNowPlayBGMName()
    {
        AudioClip nowPlayBGM = bgmSource.clip;
        if (nowPlayBGM == null)
            return null;
        else
            return nowPlayBGM.name;
    }
}