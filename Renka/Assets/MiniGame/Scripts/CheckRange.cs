using UnityEngine;
using System.Collections;

public class CheckRange : MonoBehaviour {
    [SerializeField]
    MoveBlock moveBlock;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}

    void OnTriggerEnter(Collider col)
    {

        Debug.Log("atatta");
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        if (layerName == "Player")
        {

            moveBlock.isInRange = true;
        }
    }

    
}
