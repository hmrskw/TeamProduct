using UnityEngine;
using System.Collections;

public class APIRequest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Request());
	}
	
	// Update is called once per frame
	IEnumerator Request() {
        WWW www = new WWW("http://localhost/hello.php");
        yield return www;

        string json = www.text;
        Debug.Log(json);
	
	}
}
