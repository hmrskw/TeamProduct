using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StillManager : MonoBehaviour
{

	[SerializeField]
	GameObject character1;

	[SerializeField]
	GameObject character2;

	public RawImage stillImage1;

	public RawImage stillImage2;

	//イラストのみ
	public Texture tex1;

	//イラストとテキスト付
	public Texture tex2;


	[SerializeField]
	public Button button;

	[SerializeField]
	public MemoryManager memoMana;

	public void OnClick()
	{

		Debug.Log("OnClick : " + stillImage1.texture.name);
		//memoMana.gameObject.SetActive(true);
		//gameObject.SetActive(false);

	}

	public void OnClickCharcter1()
	{

		Debug.Log("OnClickCharacter 1 ");

		character1.SetActive(false);
		character2.SetActive(true);

		gameObject.SetActive(false);
	}

	public void OnClickCharcter2()
	{

		Debug.Log("OnClickCharacter 2 ");

		character1.SetActive(true);
		character2.SetActive(false);

		
	}

}
