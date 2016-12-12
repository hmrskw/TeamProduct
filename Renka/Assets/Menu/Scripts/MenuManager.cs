using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//各キャンバスを管理して、メニューシーンをコントロールする
//ページごとにキャンパスを分けている
public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance { get; private set; }

	//メニュー内の主なページ(状態)
	public enum MenuPage
	{
		CONTENTS = 0,
		MEMORY,
		RECORD,
		PROFILE,
		CONFIG,
		RESET,
		STAFF,
		NUM,
	}

	//現在の状態
	MenuPage menu;
	public MenuPage Menu
	{ get { return menu; } }

	//ひとつ前の状態
	MenuPage prevMenu;

	//もどるボタンで戻るための記憶
	Stack<MenuPage> pageStacks = new Stack<MenuPage>();

	[SerializeField, Tooltip("このシーンに移った時最初に表示されるページ")]
	MenuPage initialPage;

	[SerializeField, Tooltip("コンテンツページ")]
	ContentsManager contents;

	[SerializeField, Tooltip("思い出ページ")]
	MemoryManager memory;

	[SerializeField, Tooltip("記録帖ページ")]
	RecordManager record;

	[SerializeField, Tooltip("人物紹介ページ")]
	ProfileManager profile;

	[SerializeField, Tooltip("設定ページ")]
	ConfigManager config;

	[SerializeField, Tooltip("リセットページ")]
	ResetManager reset;

	[SerializeField, Tooltip("スタッフページ")]
	StaffManager staff;


	//public bool IsInitEnd;

	//各マネージャーのアクティブを配列にする
	//アクティブを変更するときに呼ぶ
	delegate void Active(bool flag);
	Active[] actives;

	delegate bool BackInPage();
	BackInPage[] backs;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

	}

	void Start()
	{
		Fade.Instance.FadeOut(0.5f, null);
		//Debug.Log("actives setup");
		Init();
	}

	void Init()
	{
		//メニューのオブジェクトそれぞれ管理する
		actives = new Active[(int)MenuPage.NUM];

		//各ページのオンオフ制御
		//初期化が必要なものは処理を追加する
		actives[(int)MenuPage.CONTENTS] = (bool f) =>
		{
			contents.gameObject.SetActive(f);
		};
		actives[(int)MenuPage.MEMORY] = (bool f) => memory.gameObject.SetActive(f);
		actives[(int)MenuPage.RECORD] = (bool f) => record.gameObject.SetActive(f);
		actives[(int)MenuPage.PROFILE] = (bool f) =>
		{
			profile.gameObject.SetActive(f);
			if (!f)
			{
				profile.Select.Reset();
				profile.ResetProfiles();
			}
		};
		actives[(int)MenuPage.CONFIG] = (bool f) =>
		{
			config.StopText();
			config.gameObject.SetActive(f);
			if (!f)
			{
				config.StopText();
				config.SaveConfigData();
			}
			else
			{
				config.PlayText();
			}
		};
		actives[(int)MenuPage.RESET] = (bool f) =>
		{
			reset.gameObject.SetActive(f);
			if (f) reset.Reset();
		};
		actives[(int)MenuPage.STAFF] = (bool f) => staff.gameObject.SetActive(f);


		//初期のメニューページ
		//ResetMenu(initializePage);
		StartMenu(initialPage);
		//Reset(initialPage);

		//もどるボタンを押したときのページごとの例外処理///////////////////////
		//もどるを押しても直接コンテンツページに戻さない場合処理を書く
		backs = new BackInPage[(int)MenuPage.NUM];

		backs[(int)MenuPage.CONTENTS] = Test;
		backs[(int)MenuPage.MEMORY] = () => { return false; };
		backs[(int)MenuPage.RECORD] = () => { return false; };
		backs[(int)MenuPage.PROFILE] = () => profile.ShiftSelect();
		backs[(int)MenuPage.CONFIG] = () => { return false; };
		backs[(int)MenuPage.RESET] = () => { return false; };
		backs[(int)MenuPage.STAFF] = () => { return false; };
		///////////////////////////////////////////////////////////////////////
		//IsInitEnd = true;
	}

	public bool Test() { return false; }

	void Updata()
	{

	}

	/// <summary>
	/// ページの移動
	/// </summary>
	/// <param name="next"></param>
	public void ShiftPage(MenuPage next)
	{
		Debug.Log("ShiftPage : " + next.ToString());

		//今のページのゲームオブジェをオフにする
		actives[(int)menu](false);

		//次のページのゲームオブジェをオンにする
		actives[(int)next](true);

		prevMenu = menu;

		//ページをスタックしておく
		pageStacks.Push(menu);

		menu = next;

	}

	/// <summary>
	/// 戻るを押したときの処理
	/// </summary>
	public void ShiftBack()
	{
		Debug.Log("Shift Back");
		//Debug.Log("Stacks : " + pageStacks.Count);

		////違う処理があるかどうか
		if (backs[(int)menu]())
		{
			Debug.Log("例外");
			return;
		}

		//スタックしておいた前のページを取り出す
		var page = pageStacks.Pop();

		//もどる先が何かしら必要
		if (pageStacks.Count <= 0)
		{
			pageStacks.Push(MenuPage.CONTENTS);
		}

		//今のページのゲームオブジェをオフにする
		actives[(int)menu](false);

		//次のページのゲームオブジェをオンにする
		actives[(int)page](true);

		//
		menu = page;
	}

	/// <summary>
	/// 初期化(シーン開始時にはこれを呼ぶ)
	/// </summary>
	/// <param name="page"></param>
	public void Reset(MenuPage page)
	{
		//Awakeの初期化を行わせる
		memory.gameObject.SetActive(true);
		record.gameObject.SetActive(true);
		profile.gameObject.SetActive(true);
		config.gameObject.SetActive(true);
		reset.gameObject.SetActive(true);
		staff.gameObject.SetActive(true);

		contents.gameObject.SetActive(true);
		memory.gameObject.SetActive(false);
		record.gameObject.SetActive(false);
		profile.gameObject.SetActive(false);
		config.gameObject.SetActive(false);
		reset.gameObject.SetActive(false);
		staff.gameObject.SetActive(false);
		contents.gameObject.SetActive(false);

		if (page == MenuPage.CONTENTS) contents.gameObject.SetActive(true);
		else if (page == MenuPage.MEMORY) memory.gameObject.SetActive(true);
		else if (page == MenuPage.RECORD) record.gameObject.SetActive(true);
		else if (page == MenuPage.PROFILE) profile.gameObject.SetActive(true);
		else if (page == MenuPage.CONFIG) config.gameObject.SetActive(true);
		else if (page == MenuPage.RESET) reset.gameObject.SetActive(true);
		else if (page == MenuPage.STAFF) staff.gameObject.SetActive(true);

		pageStacks.Push(MenuPage.CONTENTS);

		menu = page;

	}

	/// <summary>
	///　指定ページを表示
	/// </summary>
	/// <param name="page"></param>
	public void ResetMenu(MenuPage page)
	{
		Debug.Log("ResetMenu : Start");
		for (int i = 0; i < (int)MenuPage.NUM; i++)
		{
			actives[i](false);
		}

		actives[(int)page](true);

		pageStacks.Push(MenuPage.CONTENTS);

		menu = page;

		Debug.Log("ResetMenu : End");
	}

	/// <summary>
	///	シーン初期時によばれる
	/// </summary>
	/// <param name="page">初期のページ</param>
	public void StartMenu(MenuPage page)
	{
		//Awakeの初期化を行わせる
		memory.gameObject.SetActive(true);
		record.gameObject.SetActive(true);
		profile.gameObject.SetActive(true);
		config.gameObject.SetActive(true);
		reset.gameObject.SetActive(true);
		staff.gameObject.SetActive(true);

		contents.gameObject.SetActive(true);
		memory.gameObject.SetActive(false);
		record.gameObject.SetActive(false);
		profile.gameObject.SetActive(false);
		config.gameObject.SetActive(false);
		reset.gameObject.SetActive(false);
		staff.gameObject.SetActive(false);
		contents.gameObject.SetActive(false);

		//var f = false;
		//contents.gameObject.SetActive(f);
		//memory.gameObject.SetActive(f);
		//record.gameObject.SetActive(f);
		//profile.gameObject.SetActive(f);
		//config.gameObject.SetActive(f);
		////if (!f) config.SaveConfigData();
		//reset.gameObject.SetActive(f);
		//staff.gameObject.SetActive(f);

		actives[(int)page](true);

		pageStacks.Push(MenuPage.CONTENTS);

		menu = page;

	}

	//ラムダ式にしたので未使用////////////////////////////////////////////////////////////////////////
	public void ActiveContents(bool isActive_)
	{
		contents.gameObject.SetActive(isActive_);
	}

	public void ActiveMemory(bool isActive_)
	{
		memory.gameObject.SetActive(isActive_);
	}

	public void ActiveRecord(bool isActive_)
	{
		record.gameObject.SetActive(isActive_);
	}

	public void ActiveProfile(bool isActive_)
	{
		profile.gameObject.SetActive(isActive_);
	}

	public void ActiveConfig(bool isActive_)
	{
		config.gameObject.SetActive(isActive_);
	}

	public void ActiveReset(bool isActive_)
	{
		reset.gameObject.SetActive(isActive_);
	}

	public void ActiveStaff(bool isActive_)
	{
		staff.gameObject.SetActive(isActive_);
	}


	//delegate bool Active(MenuPage page);


}