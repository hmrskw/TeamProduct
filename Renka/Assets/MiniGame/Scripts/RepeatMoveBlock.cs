using UnityEngine;
using System.Collections;

public class RepeatMoveBlock : MonoBehaviour {
    [SerializeField]
    float leftLimitPos;

    [SerializeField]
    float rightLimitPos;
    [SerializeField]
    bool goRight=true;
    [SerializeField]
    float speed;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(goRight==false)
        {
            if(transform.position.x>=leftLimitPos)
            {
                transform.Translate(-speed, 0, 0);
                
            }
            else
            {
                goRight = true;
            }
        }

    else
        {
            if (transform.position.x <= rightLimitPos)
            {
                transform.Translate(speed, 0, 0);

            }
            else
            {
                goRight = false;
            }
        }
	}
}
