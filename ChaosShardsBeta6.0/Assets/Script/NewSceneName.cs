using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneName : MonoBehaviour {

    public static string SceneName;

	// Use this for initialization
	void Start () {
        SceneName = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewScene(string name)
    {
        SceneName = name;
    }
}
