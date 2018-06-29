using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor_partical_system : MonoBehaviour {
    public GameObject cursorParticalSys;
    public GameObject tmp;

    private Vector3 rawposition;
    // Use this for initialization
    void Start () {
        rawposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // tmp = Instantiate(cursorParticalSys, rawposition, Quaternion.Euler(new Vector3(0, 0, 0)));
        tmp = Instantiate(cursorParticalSys, rawposition, Quaternion.Euler(new Vector3(0,0,0)));
    }
	
	// Update is called once per frame
	void Update () {
        rawposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(tmp.transform.position != rawposition)
        {
            tmp.transform.LookAt(rawposition);
            tmp.transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        }
        //tmp.transform.position = rawposition;
        //Debug.Log(tmp.transform.position);
	}
}
