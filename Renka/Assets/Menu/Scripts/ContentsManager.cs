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

		MenuManager.Instance.ShiftBack();

	}

    /// <summary>
    /// もどるボタンが押されたとき
    /// </summary>
    public void OnClickMyPage()
    {
        Debug.Log("マイページ");
        if (Fade.Instance.isFade == false)
        {

            Fade.Instance.FadeIn(1f, () => { SceneChanger.LoadScene("MyPage"); });
        }
        //SceneChanger.LoadScene("MyPage");

	}

    /// <summary>
    /// 思い出ボタンが押されたとき
    /// </summary>
    public void OnClickMemory()
    {
        //Debug.Log("思い出");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.MEMORY);
	}

    /// <summary>
    /// 記録帖ボタンが押されたとき
    /// </summary>
    public void OnClickRecord()
    {
        //Debug.Log("記録帖");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.RECORD);
	}

    /// <summary>
    /// 人物紹介ボタンが押されたとき
    /// </summary>
    public void OnClickProfile()
    {
        //Debug.Log("人物紹介");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.PROFILE);
	}

    /// <summary>
    /// 設定ボタンが押されたとき
    /// </summary>
    public void OnClickConfig()
    {
        //Debug.Log("設定");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.CONFIG);
	}

    /// <summary>
    /// リセットボタンが押されたとき
    /// </summary>
    public void OnClickReset()
    {
        //Debug.Log("リセット");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.RESET);
	}

    /// <summary>
    /// スタッフボタンが押されたとき
    /// </summary>
    public void OnClickStaff()
    {
        //Debug.Log("スタッフ");
		MenuManager.Instance.ShiftPage(MenuManager.MenuPage.STAFF);
	}
}
