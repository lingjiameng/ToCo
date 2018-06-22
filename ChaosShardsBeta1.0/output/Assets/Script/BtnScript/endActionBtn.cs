using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endActionBtn : MonoBehaviour {

    //获取控制对象
    private GameObject playerSelf;
    

    // Use this for initialization
    void Start () {
        playerSelf = GameObject.Find("self");
        

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnClick()
    {
        Debug.Log("hello");
        if(playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.Wait)
        {
            playerSelf.GetComponent<playerControl>().myState = playerControl.GameStageSate.playerEnd;
        }
    }
}
