using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour {
    public bool isInRange = false;

    [Tooltip("障害物が右から左ならTrue、左から右ならfalse")]
    [SerializeField]
    private bool isRight = true;

    [SerializeField]
    private float leftLimitPos;

    [SerializeField]
    private float rightLimitPos;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(isInRange==true)
        {
            if(isRight==true)
            {
                if(transform.position.x>=leftLimitPos)
                {
                    transform.Translate(-0.1f, 0, 0);

                }
            }

            else
            {
                if (transform.position.x <= rightLimitPos)
                {
                    transform.Translate(0.1f, 0, 0);
                }
                    
            }
        }
	}
}
