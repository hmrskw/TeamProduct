using UnityEngine;
using System.Collections;


public class KihudaPopUpButton : MonoBehaviour {
    [SerializeField]
    GameObject kihudaPopUp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PopUpEnableButton()
    {
        kihudaPopUp.SetActive(false);
        Debug.Log("PUSH!");
    }
}
