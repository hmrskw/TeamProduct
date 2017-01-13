using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class JsonParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TextAsset jsonFile = Resources.Load<TextAsset>("sample");
        Debug.Log(jsonFile.text);
        string jsonText = jsonFile.text;

        Dictionary<string,object>json=
            Json.Deserialize(jsonText) as Dictionary<string, object>;

        Debug.Log((string)json["name"]);
        Debug.Log((long)json["age"]);
        var weight =(double)json["weight"];
        var height = (double)json["height"];
        Debug.Log((string)json["bloodType"]);

        double BMI = weight / (height/100 * height/100);
        Debug.Log(BMI);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
