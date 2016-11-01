using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gage : MonoBehaviour {

    Slider _slider;
    [SerializeField]
    StageManager _stageManager;

    // Use this for initialization
    void Start () {

        _slider =GetComponent<Slider>();

    }
	
	// Update is called once per frame
	void Update () {


        _slider.value = _stageManager.gagePlayerPos;
        //Debug.Log(_stageManager.gagePlayerPos);
	}
}
