using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    public GameObject face;

    [SerializeField]
    public GameObject clothes;

    void Awake()
    {
        gameObject.SetActive(false);
    }
}
