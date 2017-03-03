using UnityEngine;
using System.Collections;

public class ChouchinRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y<=-2)
        {
            transform.eulerAngles += new Vector3(0.0f, 0.0f, 10.0f);
        }
        
    }
}
