using UnityEngine;
using System.Collections;

public class DisableCollisionFire : MonoBehaviour
{

    [SerializeField]
    GameObject[] fires;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player")
        {
            for(int i=0;i<fires.Length;i++)
            {
                fires[i].GetComponent<BoxCollider>().enabled = false;   
            }

        }

    }

}
