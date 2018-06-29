using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    public GameObject Timer;
    public bool timer_run=true;

    public float TimeCounter=0;
	// Use this for initialization
	void Start () {
        Timer.GetComponent<Text>().text = "0:00";
        timer_run = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (timer_run)
        {
            TimeCounter += Time.deltaTime;
            Timer.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}",
                (int)TimeCounter / 60, (int)TimeCounter % 60);
        }
        

    }

    public void Stop_Run()
    {
        timer_run = !timer_run;
    }
}
