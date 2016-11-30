using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//未使用
[System.Serializable]
public class FloatGage
{
	[SerializeField]
	private float max;
	public float Max
	{
		get { return max; }
		set { max = value; }
	}

	[SerializeField]
	private float val;
	public float Val
	{
		get { return val; }
		set { val = value; if (val > max) val = max; }
	}

	
	//Add
	//ChackMax;
	//
}

//[System.Serializable]
//public class ConfigData
//{
//	[SerializeField]
//	FloatGage bgm;

//	[SerializeField]
//	FloatGage se;

//	[SerializeField]
//	FloatGage voice;

//	[SerializeField]
//	FloatGage textBox;

//	[SerializeField]
//	FloatGage textSpd;

//	[SerializeField]
//	bool isSkip;
//}

/// <summary>
/// コンフィグデータ
/// ※データマネージャーに保存しておく
/// </summary>
[System.Serializable]
public class ConfigData
{
	[SerializeField, Range(0f, 1f), Tooltip("BGMの大きさ")]
	float bgm;
	public float BGM { get { return bgm; } set { bgm = value; } }

	[SerializeField, Range(0f, 1f), Tooltip("SEの大きさ")]
	float se;
	public float SE { get { return se; } set { se = value; } }

	[SerializeField, Range(0f, 1f), Tooltip("VOICEの大きさ")]
	float voice;
	public float VOICE { get { return voice; } set { voice = value; } }

	[SerializeField, Range(0f, 1f), Tooltip("テキストボックスの透明度")]
	float textBox;
	public float TextBox { get { return textBox; } set { textBox = value; } }

	[SerializeField, Range(0f, 1f), Tooltip("テキストスピード")]
	float textSpd;
	public float TextSpd { get { return textSpd; } set { textSpd = value; } }

	[SerializeField, Tooltip("既読スキップの有無")]
	bool isSkip;
	public bool IsSkip { get { return isSkip; } set { isSkip = value; } }
}


public class ConfigManager : MonoBehaviour
{

	[SerializeField, Tooltip("設定のデータ")]
	ConfigData config;

	[SerializeField, Tooltip("BGMのスライダーを参照させる")]
	Slider bgm;

	[SerializeField, Tooltip("SEのスライダーを参照させる")]
	Slider se;

	[SerializeField, Tooltip("VOICEのスライダーを参照させる")]
	Slider voice;

	[SerializeField, Tooltip("テキストボックスのスライダーを参照させる")]
	Slider textBox;

	[SerializeField, Tooltip("テキストスピードのスライダーを参照させる")]
	Slider textSpd;

	[SerializeField, Tooltip("既読スキップ(有)のトグルを参照させる")]
	Toggle enableSkip;

	[SerializeField, Tooltip("既読スキップ(無)のトグルを参照させる")]
	Toggle disableSkip;

	[SerializeField, Tooltip("テキストエリアの画像を参照させる")]
	Image textAreaImage;

	[SerializeField, Tooltip("確認テキストを参照させる")]
	Text text;

	//[SerializeField, Range(0.5f, 0f), Tooltip("テキスト速さ")]
	//float testTextSpeed2;

	[SerializeField, Range(0f, 2f), Tooltip("テキストごとの間隔")]
	float testTextIntervalSpeed;

	//描画するテキスト
	string testText = "";

	void Awake()
	{
		//Debug.Log("Awake Config");
		//画面のテキストから描画するテキスト移す
		testText = text.text;
	}

	void Start()
	{
		Debug.Log("Start Config");

		//コンフィグデータを読み込む
		//※データマネージャーのデータ
		LoadConfigData(config);

		//テキスストボックスのバーがいじられたときのアルファの変更
		textBox.onValueChanged.AddListener((float value) =>
	   {
		   var a = textAreaImage.color;
		   a.a = value;
		   textAreaImage.color = a;
	   });
		//textBox.onValueChanged.AddListener(OnSlide);
	}

	/// <summary>
	/// テキストボックスのスライダーを動かした時の処理.
	/// </summary>
	/// <param name="value"></param>
	public void OnSlide(float value)
	{
		//Debug.Log("OnSlide : " + value);
		var a = textAreaImage.color;
		a.a = value;
		textAreaImage.color = a;
		//Debug.Log("Color : " + textAreaImage.color);
	}

	/// <summary>
	/// コンフィグデータを読み込んでUIに反映させる
	/// </summary>
	/// <param name="data">コンフィグデータ</param>
	public void LoadConfigData(ConfigData data)
	{
		Debug.Log("LoadConfig");
		bgm.value = data.BGM;
		se.value = data.SE;
		voice.value = data.VOICE;
		textBox.value = data.TextBox;
		textSpd.value = data.TextSpd;
		if (data.IsSkip)
		{
			enableSkip.isOn = true;
			//disableSkip.isOn = false;
		}
		else
		{
			//enableSkip.isOn = false;
			disableSkip.isOn = true;
		}
	}

	/// <summary>
	/// 現在のUIからコンフィグデータを生成する
	/// </summary>
	/// <returns>コンフィグデータ</returns>
	ConfigData CreateConfigData()
	{
		var data = new ConfigData();
		data.BGM = bgm.value;
		data.SE = se.value;
		data.VOICE = voice.value;
		data.TextBox = textBox.value;
		data.TextSpd = textSpd.value;
		if (enableSkip.isOn)
		{
			data.IsSkip = true;
		}
		else
		{
			data.IsSkip = false;
		}

		return data;
	}

	/// <summary>
	/// データマネージャーにコンフィグ情報を保存する
	/// </summary>
	public void SaveConfigData()
	{
		Debug.Log("SaveConfigData");
		//var data = CreateConfigData();
	}


	/// <summary>
	/// 再生の確認
	/// </summary>
	public void PlayText(int id)
	{
		Debug.Log("PlayText : " + id);
	}

	bool IsStopText;

	/// <summary>
	/// テキスト再生させるコルーチン
	/// </summary>
	public void PlayText()
	{

		Debug.Log("PlayText()");
		//StartCoroutine(PlayTextCoroutine(testText));
		//var speed = testTextSpeed * Scale(new Range<float>(0f, 100f), new Range<float>(0f, 1f));
		//var speed = ScaleFig(testTextSpeed, 0f, 100f, 0, 1);
		//Debug.Log("SpeedF : " + speed);

		StartCoroutine(PlayTextCoroutine2(testText, 0, testTextIntervalSpeed));
	}

	/// <summary>
	/// 再生ボタンが押されたとき呼ばれる
	/// </summary>
	/// <param name="id">BGM:0, SE:1, VOICE:2</param>
	public void PlaySound(int id)
	{
		Debug.Log("PlaySound ID:" + id);
	}

	////未使用
	IEnumerator PlayTextCoroutine(string str)
	{
		//Debug.Log("TextCroutine");

		var strCnt = 0;
		var strMax = str.Length;

		while (IsStopText == false)
		{
			strCnt++;

			if (strCnt >= strMax) strCnt = 0;

			yield return null;

			//Debug.Log("Coroutine Update");

			var outStr = str.Substring(0, strCnt);

			text.text = outStr;
		}

		IsStopText = false;
	}

	/// <summary>
	/// テキスト読み上げる(ループ)
	/// ※外部でIsStopがtrueになるまで
	/// </summary>
	/// <param name="str">読み上げるテキスト</param>
	/// <param name="textSpeed">(未使用)テキスト速度</param>
	/// <param name="intervalSpeed">(未使用)テキスト間の時間</param>
	/// <returns></returns>
	IEnumerator PlayTextCoroutine2(string str, float textSpeed, float intervalSpeed)
	{

		Debug.Log("TextSpeed : " + (1 - textSpd.value));
		var strCnt = 0;
		var strWaitCnt = 0;
		var strMax = str.Length;

		while (IsStopText == false)
		{

			strCnt++;

			if (strCnt >= strMax)
			{
				strCnt = 0;
				//yield return new WaitForSeconds(intervalSpeed);
				yield return new WaitForSeconds(testTextIntervalSpeed);
			}
			else
			{
				//yield return new WaitForSeconds(textSpeed);
				//yield return new WaitForSeconds( 1f - textSpd.value );
				yield return new WaitForSeconds(ScaleNormalized(1 - textSpd.value, 1f, 0.1f));
			}

			//Debug.Log("Coroutine Update");

			var outStr = str.Substring(0, strCnt);

			text.text = outStr;
		}

		IsStopText = false;
	}

	public void StopText()
	{
		StopCoroutine(PlayTextCoroutine2("end",0f,0f));
		IsStopText = false;
	}


	public void ResetPlayText()
	{
		IsStopText = false;
	}

	/// <summary>
	/// ある値(範囲a)を範囲bの数に拡大拡縮する
	/// </summary>
	/// <param name="value"></param>
	/// <param name="valMin"></param>
	/// <param name="valMax"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	float ScaleFig(float value, float valMin, float valMax, float min, float max)
	{
		var baseWidth = valMax - valMin;
		var width = max - min;

		//value : ans == valWid : ansWid
		var ans = value * width / baseWidth + min;

		return ans;
	}

	float ScaleNormalized(float value, float valMax, float max)
	{
		return value * max / valMax;
	}

	float Scale(Range<float> val, Range<float> scl)
	{
		return (scl.max - scl.min) / (val.max - val.min);
	}

	public class Range<Type>
	{
		public Type max;
		public Type min;

		public Range(Type x, Type n)
		{
			max = x;
			min = n;
		}
	}
}
