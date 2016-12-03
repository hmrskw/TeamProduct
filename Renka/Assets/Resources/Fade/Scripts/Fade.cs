/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Fade : MonoBehaviour
{
    private static Fade instance;

    public static Fade Instance
    {
        get { return instance; }
    }

    IFade fade;

    float cutoutRange;

    public bool isFade { get; private set; }

    FadeImage fadeImage;

    [SerializeField]
    Texture[] MaskTexture;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start ()
	{
		Init ();
        fade.Range = cutoutRange;
    }

	void Init ()
	{
        fadeImage = GetComponent<FadeImage>();
        fade = GetComponent<IFade> ();
	}

	void OnValidate ()
	{
		Init ();
		fade.Range = cutoutRange;
	}

    public void SetR(float num_)
    {
        cutoutRange = num_;
    }

    IEnumerator FadeoutCoroutine (float time, System.Action action)
	{
        isFade = true;
        //float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);
        float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

        var endFrame = new WaitForEndOfFrame ();

        while (Time.timeSinceLevelLoad <= endTime) {
			cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 0;
		fade.Range = cutoutRange;

		if (action != null) {
			action ();
		}
        isFade = false;
    }

    IEnumerator FadeinCoroutine (float time, System.Action action)
	{
        isFade = true;
        float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);
		
		var endFrame = new WaitForEndOfFrame ();

		while (Time.timeSinceLevelLoad <= endTime) {
			cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 1;
		fade.Range = cutoutRange;

		if (action != null) {
			action ();
		}
        isFade = false;
    }

    /*シーンまたぐとフェードアウトの開始が遅れるから使わない
    IEnumerator FadeInFadeOutCoroutine(float time, System.Action action)
    {
        yield return StartCoroutine(FadeinCoroutine(time, action));
        yield return StartCoroutine(FadeoutCoroutine(1, null));
    }

    public Coroutine FadeInOut(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeInFadeOutCoroutine(time, action));
    }*/

    public Coroutine FadeOut (float time, System.Action action)
	{
		StopAllCoroutines ();
        return StartCoroutine (FadeoutCoroutine (time, action));
	}

    public Coroutine FadeOut (float time,int id)
	{
        SetMaskTexture(id);
        return FadeOut (time, null);
	}

	public Coroutine FadeIn (float time, System.Action action)
	{
		StopAllCoroutines ();
		return StartCoroutine (FadeinCoroutine (time, action));
	}

	public Coroutine FadeIn (float time, int id)
	{
        SetMaskTexture(id);
        return FadeIn (time, null);
	}

    public void SetMaskTexture(int MaskTextureID)
    {
        fadeImage.UpdateMaskTexture(MaskTexture[MaskTextureID]);
    }
}