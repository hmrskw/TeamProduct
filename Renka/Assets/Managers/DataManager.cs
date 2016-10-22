using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
    private static DataManager instance;

    public static DataManager Instance
    {
        get { return instance; }
    }

    public int likeabillity { set; get; }

    public int endLine { set; get; }

    //シーンまたいでもオブジェクトが破棄されなくする
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        init();
    }

    public void init()
    {
        likeabillity = 0;
        endLine = 0;
    }
}
