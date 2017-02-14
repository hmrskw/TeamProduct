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

	[SerializeField, Tooltip("キャラ選択ウィンドウ")]
	GameObject window4;

	[SerializeField, Tooltip("キャラ選択確認ウィンドウ")]
	GameObject window5;

	void Start()
	{

		Reset();

	}

	//ゲームオブジェクトのリセット
	public void Reset()
	{
		//各ウィンドウの初期状態
		window1.SetActive(false);
		window2.SetActive(false);
		window3.SetActive(false);
		window4.SetActive(true);
		window5.SetActive(false);

		characterIDBuffer = -1;
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
		DataManager.Instance.nowReadChapterID = -1;

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

	//変更したいキャラのIDを一時的に覚えておく
	int characterIDBuffer;

	//キャラクター選択
	public void OnClickCharacter(int CharacterID_)
	{
		characterIDBuffer = CharacterID_;

		//キャラ変更確認画面へ
		window4.SetActive(false);
		window5.SetActive(true);

	}

	//キャラ変更する
	public void OnClickChangeCharacterYes()
	{
		Debug.Log("ウィンドウ⑤　Yes");

		if (Fade.Instance.isFade == true) return;

		DataManager.Instance.masteringData.masteringCharacterID = characterIDBuffer;
		//ConvertADVData.Instance.SetMasteringCharacterLastStoryID();
		//DataManager.Instance.masteringData.masteringCharacterLastStoryID = 1;
		//DataManager.Instance.masteringData.masteringCharacterLastChapterID = 0;
		DataManager.Instance.nowReadStoryID = 0;
		DataManager.Instance.nowReadChapterID = 0;
		DataManager.Instance.endLine = 1;
		DataManager.Instance.masteringData.likeabillity = 0;

		Debug.Log("キャラクターデータの変更完了");

		SaveData.SaveMasteringData();

		Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MyPage"); });
	}

	//キャラ変更しない
	public void OnClickChangeCharacterNo()
	{
		Debug.Log("ウィンドウ⑤　No");

		//コンテンツ画面に戻る？
		MenuManager.Instance.ShiftBack();
	}


}
