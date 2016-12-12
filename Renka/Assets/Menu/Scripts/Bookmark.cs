using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Bookmark : MonoBehaviour
{

	[SerializeField]
	string name;

	[SerializeField, Tooltip("栞の名前")]
	public Image image;

	[SerializeField, Tooltip("栞のボタンスクリプト")]
	public Button button;

	[SerializeField, Tooltip("栞の名前の画像")]
	public RawImage imageName;

	public void OnClick()
	{
		Debug.Log("栞がクリックされた : " + name );
	}


}
