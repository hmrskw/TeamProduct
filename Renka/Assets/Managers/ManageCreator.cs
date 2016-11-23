using UnityEngine;
using System.Collections;

public class ManageCreator : MonoBehaviour {

    //Awakeより前に自動でマネージャーを作成する
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreatorManager()
    {
#if UNITY_EDITOR
        Screen.SetResolution(450, 800, false, 60);
#elif UNITY_ANDROID
        Screen.fullScreen = true;
#endif

        //Cursor.visible = false;
        //Instantiate(this.gameObject);
        GameObject ManagerObj = Instantiate((GameObject)Resources.Load("Fade/FadeCanvas"));
        //GameObject ManagerObj = new GameObject("Manager", typeof(DataManager),typeof(InputManager));
        //ManagerObj.AddComponent(typeof(DataManager)); //= new GameObject("Manager", typeof(DataManager),typeof(InputManager));
        //ManagerObj.AddComponent(typeof(InputManager));
        DontDestroyOnLoad(ManagerObj);
    }
}
