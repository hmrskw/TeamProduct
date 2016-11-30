using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//自身のコンポーネントを参照
public class Memory : MonoBehaviour
{
	//子の画像
	[SerializeField]
	public RawImage[] images;

	public void ButtonClick( Texture tex )
	{
		Debug.Log("ButtonClick : " + tex.name);
	}

	public void ButtonClick(int id)
	{
		Debug.Log("ButtonClick : " + images[id].texture.name);
	}

}
