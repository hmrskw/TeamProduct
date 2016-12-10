using UnityEngine;
using System.Collections;

public class CheckRange : MonoBehaviour {
    [SerializeField]
    GameObject moveTarget;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        if (layerName == "CheckRange")
        {
            
        }

    }
}
