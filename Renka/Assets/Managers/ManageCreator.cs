using UnityEngine;
using System.Collections;

public class ManageCreator : MonoBehaviour {

    //Awakeより前に自動でマネージャーを作成する
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreatorManager()
    {
        Screen.SetResolution(540, 960, false, 60);
        //Cursor.visible = false;
        GameObject ManagerObj = new GameObject("Manager", typeof(DataManager),typeof(InputManager));
        DontDestroyOnLoad(ManagerObj);
    }
}
