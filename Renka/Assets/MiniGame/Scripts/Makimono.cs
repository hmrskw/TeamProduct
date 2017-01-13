using UnityEngine;
using System.Collections;



public class Makimono : MonoBehaviour {
    [SerializeField]
    float makimonoLimitPosY;
    [SerializeField]
    float scrollSpeed;

    

      
    

    public IEnumerator MakimonoScroll()
{
        
        while (transform.localPosition.y >= makimonoLimitPosY)
        {
            transform.Translate(0, scrollSpeed, 0);
            yield return null;
        }
        yield return null;
    }
    public IEnumerator resultMakimonoScroll()
    {

        while (transform.localPosition.y >= makimonoLimitPosY)
        {
            transform.Translate(0, scrollSpeed, 0);
            yield return null;
        }
        yield return null;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
