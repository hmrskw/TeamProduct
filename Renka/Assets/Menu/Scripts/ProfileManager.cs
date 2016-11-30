using UnityEngine;
using System.Collections;

public class ProfileManager : MonoBehaviour
{
	/// <summary>
	/// プロファイルで選択できるページごとに
	/// </summary>
	public enum ProfileName
	{
		NEGATIVE = -1,
		TATSUMI = 0,
		YUUSUKE,
		UTAROU,
		KAZUMA,
		OTHER,
		NUM,
	}

    //[SerializeField]
    //ProfileImage[] ProfileScripts;

	[SerializeField, Tooltip("selectの参照")]
	ProfileSelect select;
	public ProfileSelect Select
	{
		get { return select; }
		private set { select = value; }
	}

	[SerializeField, Tooltip("選択時に表示される方のオブジェクトのスクリプト")]
	GameObject[] profileObjects;


	//現在セレクトかどうか
	//select : true
	//profile : false
	bool isSelect;

	void Start()
	{
		ResetProfiles();
	}

	/// <summary>
	/// プロファイルの状態をすべてオフにして
	/// プロファイル選択画面を表示する
	/// </summary>
	public void ResetProfiles()
	{
		//全てアクティブをfalseにして非表示にしておく
		for (int i = 0; i < profileObjects.Length; i++)
		{
			profileObjects[i].SetActive(false);
		}

		Select.gameObject.SetActive(true);
		isSelect = true;
	}

	/// <summary>
	/// プロファイル選択からプロファイルに映る
	/// </summary>
	/// <param name="name"></param>
	void ActiveProfile( ProfileName name)
	{
		Debug.Log("ActiveProfile");
		//false
		Select.gameObject.SetActive(false);

		//true
		profileObjects[(int)name].SetActive(true);

		//isSelect
		isSelect = false;

	}

	/// <summary>
	/// キャラクターのプロフィールからプロフィールの選択画面に行く処理
	/// </summary>
	/// <returns></returns>
	public bool ShiftSelect()
	{
		Debug.Log("Shift Select");
		if (isSelect) return false;

		ResetProfiles();

		return true;
	}

    /// <summary>
    /// UI右の画像がクリックされたとき
    /// </summary>
    public void OnClickLeft()
    {
        Debug.Log("Button ←");
    }

    /// <summary>
    /// UI左の画像がクリックされたとき
    /// </summary>
    public void OnClickRight()
    {
        Debug.Log("Button →");
    }
	
	/// <summary>
	/// プロファイルがクリックで選択された
	/// </summary>
	/// <param name="id"></param>
	public void OnClickProfile(int id)
	{
		ProfileName name = (ProfileName)id;
		Debug.Log("OnClickProfile : " + name.ToString());

		if (Select.IsHold) return;

		//セレクトをオフに
		Select.gameObject.SetActive(false);

		//プロファイルをオンに
		ActiveProfile((ProfileName)id);

	}

}
