using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour {

    public RawImage bgimages;
    private float timecounter = 0;
    private float SumTime = 1;
    private bool show = false;
    private bool trigger = false;
    private bool nextscene = false;

    // Use this for initialization
    void Start () {
        startshow();

        Invoke("endshow", 3);
    }

    void startshow()
    {
        trigger = true;
        show = true;
        timecounter = 0;
    }

    void endshow()
    {
        trigger = true;
        show = false;
        timecounter = 0;

    }
	
	// Update is called once per frame
	void Update () {

        if(nextscene)
            SceneManager.LoadScene("main");

        if (!trigger)
            return;

        timecounter += Time.deltaTime;

        if (show)
        {
            bgimages.color = new Color(1, 1, 1, (timecounter / SumTime));
            if (timecounter >= SumTime)
                trigger = false;
        }
        else
        {
            bgimages.color = new Color(1, 1, 1, 1 - (timecounter / SumTime));
            if (timecounter >= SumTime)
            {
                trigger = false;
                nextscene = true;
            }
        }

    }
}
