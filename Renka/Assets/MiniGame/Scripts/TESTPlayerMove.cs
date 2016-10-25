using UnityEngine;
using System.Collections;

public class TESTPlayerMove : MonoBehaviour
{

    [SerializeField]
    float playerMoveSpeed;
    [SerializeField, Range(-2.5f, -1.5f)]
    float leftLimitPos;
    [SerializeField, Range(1.5f, 2.5f)]
    float rightLimitPos;
    [SerializeField]
    Player player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (player.hp > 0)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= leftLimitPos)
            {
                transform.Translate(horizontal * playerMoveSpeed, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x <= rightLimitPos)
            {
                transform.Translate(horizontal * playerMoveSpeed, 0, 0);
            }


        }
    }
}
