using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    [SerializeField]
    GameObject goalPopUp;

    public bool isGoal=false;
        
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player")
        {
            goalPopUp.SetActive(true);
            isGoal = true;
            

        }


    }
}
