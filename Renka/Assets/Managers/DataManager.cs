using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
    private static DataManager instance;

    public static DataManager Instance
    {
        get { return instance; }
    }

    //好感度ポイント
    //int likeabillity;

    public int likeabillity { set; get; }

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
    }
}
