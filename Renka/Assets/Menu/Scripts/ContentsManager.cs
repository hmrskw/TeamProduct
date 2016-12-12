using UnityEngine;
using System.Collections;

public class ContentsManager : MonoBehaviour
{

	/// <summary>
	/// もどるボタンが押されたとき
	/// </summary>
	public void OnClickBack()
	{
		//Debug.Log("もどる");

		//MenuManager.Instance.ShiftBack();
		if (Fade.Instance.isFade == false && MenuManager.Instance.Menu != MenuManager.MenuPage.CONTENTS)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftBack(); Fade.Instance.FadeOut(0.5f, null); });

	}

	/// <summary>
	/// もどるボタンが押されたとき
	/// </summary>
	public void OnClickMyPage()
	{
		Debug.Log("マイページ");
		if (Fade.Instance.isFade == false)
		{

			Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); Fade.Instance.FadeOut(0.5f, null); });
		}
		//SceneChanger.LoadScene("MyPage");

	}

	/// <summary>
	/// 思い出ボタンが押されたとき
	/// </summary>
	public void OnClickMemory()
	{
		//Debug.Log("思い出");
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.MEMORY); Fade.Instance.FadeOut(0.5f, null); });

	}

	/// <summary>
	/// 記録帖ボタンが押されたとき
	/// </summary>
	public void OnClickRecord()
	{
		//Debug.Log("記録帖");
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.RECORD); Fade.Instance.FadeOut(0.5f, null); });
	}

	/// <summary>
	/// 人物紹介ボタンが押されたとき
	/// </summary>
	public void OnClickProfile()
	{
		//Debug.Log("人物紹介");
		//MenuManager.Instance.ShiftPage(MenuManager.MenuPage.PROFILE);
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.PROFILE); Fade.Instance.FadeOut(0.5f, null); });
	}

	/// <summary>
	/// 設定ボタンが押されたとき
	/// </summary>
	public void OnClickConfig()
	{
		//Debug.Log("設定");
		//MenuManager.Instance.ShiftPage(MenuManager.MenuPage.CONFIG);
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.CONFIG); Fade.Instance.FadeOut(0.5f, null); });
	}

	/// <summary>
	/// リセットボタンが押されたとき
	/// </summary>
	public void OnClickReset()
	{
		//Debug.Log("リセット");
		//MenuManager.Instance.ShiftPage(MenuManager.MenuPage.RESET);
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.RESET); Fade.Instance.FadeOut(0.5f, null); });
	}

	/// <summary>
	/// スタッフボタンが押されたとき
	/// </summary>
	public void OnClickStaff()
	{
		//Debug.Log("スタッフ");
		//MenuManager.Instance.ShiftPage(MenuManager.MenuPage.STAFF);
		if (Fade.Instance.isFade == false)
			Fade.Instance.FadeIn(0.5f, () => { MenuManager.Instance.ShiftPage(MenuManager.MenuPage.STAFF); Fade.Instance.FadeOut(0.5f, null); });
	}
}
