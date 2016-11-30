using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class Bookmarks : MonoBehaviour
{
	[SerializeField]
	Bookmark[] bookMarks;

	void Start ()
	{
		bookMarks[0].imageName.enabled = true;
	}

}
