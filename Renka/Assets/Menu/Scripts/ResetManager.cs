using UnityEngine;
using System.Collections;

public class ResetManager : MonoBehaviour
{

	[SerializeField, Tooltip("確認ウィンドウ")]
	GameObject window1;

	[SerializeField, Tooltip("再確認ウィンドウ")]
	GameObject window2;

	[SerializeField, Tooltip("リセットを終えたウィンドウ")]
	GameObject window3;

	void Start()
	{

		Reset();

	}

	//ゲームオブジェクトのリセット
	public void Reset()
	{
		//各ウィンドウの初期状態
		window1.SetActive(true);
		window2.SetActive(false);
		window3.SetActive(false);
	}


	public void OnClickWindow1Yes()
	{
		Debug.Log("ウィンドウ①　Yes");

		//二つ目のウィンドウへ
		window1.SetActive(false);
		window2.SetActive(true);

	}

	public void OnClickWindow1No()
	{
		Debug.Log("ウィンドウ①　No");

		//コンテンツ画面に戻る？
		MenuManager.Instance.ShiftBack();
	}

	public void OnClickWindow2Yes()
	{
		Debug.Log("ウィンドウ②　Yes");

        //削除
        SaveData.ResetMasteringData();
        SaveData.ResetFinishedStoryData();
        DataManager.Instance.masteringData = new DataManager.MasteringData();
        for (int i = 0; i < 4; i++)
        {
            DataManager.Instance.finishedStoryData[i] = new DataManager.FinishedStoryData();
        }
        DataManager.Instance.nowReadStoryID = -1;

        //三つ目の画面へ
        window2.SetActive(false);
		window3.SetActive(true);
	}

	public void OnClickWindow2No()
	{
		Debug.Log("ウィンドウ②　No");

		//一つ目の画面へ
		window2.SetActive(false);
		window1.SetActive(true);


	}


}
