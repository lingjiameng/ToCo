using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    public GameObject Timer;

    private float TimeCounter=0;
	// Use this for initialization
	void Start () {
        Timer.GetComponent<Text>().text = "0:00";
	}
	
	// Update is called once per frame
	void Update () {
        TimeCounter+=Time.deltaTime;
        Timer.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}",
            (int)TimeCounter / 60, (int)TimeCounter % 60);

    }
}
